clc; clear all; close all;

moviesFile = fopen('TagGenome/movies.bigdat');
mov = textscan(moviesFile,'%d\t%s\t%f64\n','Delimiter','\t');
mov{1} = double(mov{1});
m = [mov{1} mov{3}];
nm = size(m,1);
fclose(moviesFile);

tagsFile = fopen('TagGenome/tags.bigdat');
tag = textscan(tagsFile,'%d\t%s\t%f64\n','Delimiter','\t');
t = [tag{1} tag{3}];
nt = size(t,1);
fclose(tagsFile);

%% Attempt corr experiment

% tagsMoviesFile = fopen('TagGenome/tag_relevance.bigdat');
% tagmov = textscan(tagsMoviesFile,'%d\t%d\t%f64\n','Delimiter','\t');
% mt = [tagmov{1} tagmov{2} tagmov{3}];
% mtv = tagmov{3};
% %mtv = zscore(mtv);
% nmt = size(mt,1);
% fclose(tagsMoviesFile);

% ids = sortrows(t,2);
% ids = ids(:,1);
% ids = flipud(ids);
% names = tag{2}(ids+1);
% 
% nidscor = 5;
% corr = zeros(nt,nidscor+1);
% corr(:,1) = t(:,1);
% curvals = zeros(nidscor,1);
% 
% for ii = 1:(nmt/nm)
%     ii
%     for jj = 1:nidscor
%         currFeatId = ids(jj);
%         curvals(jj) = mt(ii+currFeatId);
% 
%     end
%     for kk = 1:nt
%         for jj = 1:nidscor
%            if(ids(jj) == kk-1)
%                continue;
%            end
%            corr(kk,jj+1) = corr(kk,jj+1)...
%                + (curvals(jj) - mtv(ii+kk-1))^2 * double(m(ii,2));
%         end
%     end
% end

%% Attempt some magic feature selection

f = 10;
k = 200; % Number of movies in top k
kdisp = 20; % Number of movies/tags to display
w = [0 2 1 6 2 .2]; % wtag wnormk wnormover wno_norm varPenalk varPenalOver

% wtag: Popularity of tag. Add bias toward popular tags.
% wnormk: Normalized weight to top k. High discovery of tags unique to class
% wnormover: Normalized weight to top k. Some bias to tags like 'original'
% wno_norm: Actual weight. Inherantly biased towards popular tags
% varPenalk: Penalize tags with high variance within top k.
%   ex: star-wars, indian-jones, pixar etc..
% varPenalOver: Penalize tags with high variance overall.

M = csvread('TagGenome/features.csv');
featsum = sum(M);
featvar = var(M);

auc = csvread('autoencode/pcTorchMore.csv');
auc = auc(:,1:f); %eliminate extra row
auc = (auc + 1) / 2; % -1 -> 1 to 0 -> 1
auc = auc - repmat(min(auc),nm,1); % shift actual min to zero
auc = auc ./ repmat(max(auc),nm,1); % scale actual max to 1

tmp = double(mov{1});
auc = [(1:length(tmp))' tmp auc];

aucsum = sum(auc);

tags_norm = zeros(1,nt);

% Get average tag weights in top K for normalization
for ii = 3:(f+2)
    m_sort = flipud(sortrows(auc,ii));
    topk = m_sort(1:k);
    m_names = mov{2}(m_sort(:,1));
    m_feat = M(topk,:);
    tags_norm = tags_norm + sum(m_feat);
end
tags_norm = tags_norm / f;

for ii = 3:(f+2)
    m_sort = flipud(sortrows(auc,ii));
    topk = m_sort(1:k,1);
    m_names = mov{2}(m_sort(:,1));
    m_feat = M(topk,:);
    m_feat = m_feat .* repmat(m_sort(1:k,ii),1,nt); % weigh by activation
    m_feat_sum = [(1:nt)' ...
        ( (w(1)*tag{3}./max(tag{3}))'...
        + w(2)*sum(m_feat)./tags_norm...
        + w(3)*sum(m_feat)./featsum...
        + w(4)*mean(m_feat)...
        - w(5)*var(m_feat)...
        - w(6)*featvar)'];
    t_sort = flipud(sortrows(m_feat_sum,2));
    t_names = tag{2}(t_sort(:,1));
    ii-2
    t_names(1:kdisp)
    m_names(1:kdisp)
end

%% Write features matrix out to csv file
% 
% movieIDs = containers.Map({0},{0});
% 
% count = 1;
% for ii = 1:size(mt,1)
%     ID = mt(ii,1);
%     if isKey(movieIDs,ID) == 0 
%        movieIDs = [movieIDs; containers.Map({ID},{count})];
%        count = count + 1
%     end
% end
% %%
% features = zeros(9734,1128);
% 
% h = waitbar(0,'Grab a Coffee')
% 
% for ii = 1:size(mt,1) 
%     movieId = movieIDs(mt(ii,1));
%     tagId = mt(ii,2) + 1;
%     tagW = mtv(ii); % tag weight/relevance
%     features(movieId,tagId) = tagW;
%     waitbar(ii/size(mt,1),h,sprintf('%f',ii/size(mt,1)))
% end
% 
% clearvars -except features
% csvwrite('features.csv',features)

%% Output for the server

featnames = {...
    'f1',...
    'f2'...
    'f3',...
    'f4',...
    'f5',...
    'f6',...
    'f7',...
    'f8',...
    'f9',...
    'f10'...
    };

auc = sortrows(auc,1);
feats = auc(:,2:end);

outp = fopen('outp.txt','w');
for ii=1:length(featnames)
    if(ii < length(featnames))
        fprintf(outp,'%s~',featnames{ii});
    else
        fprintf(outp,'%s\n',featnames{ii});
    end
end

for ii=1:nm
    fprintf(outp,'%d',feats(ii,1));
    for jj=2:size(feats,2)
       fprintf(outp,'~%f',feats(ii,jj)); 
    end
    fprintf(outp,'\n');
end
fclose(outp);
