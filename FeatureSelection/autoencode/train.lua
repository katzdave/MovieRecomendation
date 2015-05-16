parameters,gradParameters = auto:getParameters()

local feval = function(x)
     -- get new parameters
    if x ~= parameters then
    	parameters:copy(x)
    end
    gradParameters:zero()
    local f = 0;

    for i = 1,#inputs do
    	local oHat = auto:forward(inputs[i])
    	f = f + criterion:forward(oHat,inputs[i])
    	auto:backward(inputs[i],criterion:backward(oHat,inputs[i]))
	end

    print('Error:',f)
	return f,gradParameters
end

sgdconf = {
	-- learningRate = 1e-3,
	learningRate = 0.5e-3,
	learningRateDecay = 0,
    momentum = 0.9
}

-- if epoch > 160 then
-- 	sgdconf.learningRate = 1e-4
-- end	

_, err = optim.nag(feval, parameters, sgdconf)

errTrail[epoch] = err[1]

gnuplot.pdffigure('test.pdf')
gnuplot.grid(true)
gnuplot.axis({0,600,50,500})
gnuplot.title('Reconstruction Error')
gnuplot.xlabel('Epoch')
gnuplot.ylabel('MSE')
gnuplot.plot({errTrail[{{1,epoch}}],'-'})
gnuplot.plotflush()

epoch = epoch + 1
