file = io.open('../featuresMunged.csv',r)

--[[
	featuresMunged is a space delimited file generated from the
	features.csv with:
	cat features.csv | sed 's/,/ /g'> featuresMunged.csv
]]

inputs = {}

local nColumns = 1128

while true do
	local line = file:read('*line')
	if line == nil then break end

	local lineTensor = torch.Tensor(nColumns)
	local target = torch.Tensor(1)

	local count = 1
	for i in string.gmatch(line, '%S+') do
		lineTensor[count] = tonumber(i)
		count = count + 1
	end

    table.insert(inputs,lineTensor:cuda())
end

file:close()