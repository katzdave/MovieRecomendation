f = io.open('pcTorchMore2.csv','w')

for i = 1,#inputs do
	principalComponents = encode:forward(inputs[i])
	for j = 1,principalComponents:numel() do
		f:write(tostring(principalComponents[j]))
		-- if not j == principalComponents:numel() then 
			f:write(',')
		-- end
	end
	f:write('\n')
end

f:close()