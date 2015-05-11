require 'nn'
require 'cunn'
require 'cutorch'
require 'unsup'
require 'gnuplot'

encode = nn.Sequential()
encode:add(nn.Linear(1128,1128))
encode:add(nn.Tanh())
encode:add(nn.Linear(1128,10))
encode:add(nn.Tanh())
encode:add(nn.Diag(10))
encode:cuda()

decode = nn.Linear(10,1128)
decode:cuda()

auto = nn.Sequential()
auto:add(encode)
auto:add(decode)

criterion = nn.MSECriterion()
criterion:cuda()

dofile('loadData.lua')

epoch = 1
nEpochs = 1e3
errTrail = torch.Tensor(nEpochs)

for i = 1,200 do
	dofile('train.lua')
end

dofile('writeCSV.lua')