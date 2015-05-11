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
	learningRate = 1e-3,
	learningRateDecay = 0,
    momentum = 0.9
}

_, err = optim.nag(feval, parameters, sgdconf)

errTrail[epoch] = err[1]

gnuplot.grid(true)
gnuplot.title('Reconstruction Error')
gnuplot.xlabel('Epoch')
gnuplot.ylabel('MSE')
gnuplot.plot({errTrail[{{1,epoch}}],'-'})

epoch = epoch + 1
