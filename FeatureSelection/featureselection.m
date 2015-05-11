clc; clear all; close all;

moviesFile = fopen('TagGenome/movies.bigdat');
mov = textscan(moviesFile,'%d\t%s\t%f64\n','Delimiter','\t');
m = [mov{1} mov{3}];
nm = size(m,1);
fclose(moviesFile);

tagsFile = fopen('TagGenome/tags.bigdat');
tag = textscan(tagsFile,'%d\t%s\t%f64\n','Delimiter','\t');
t = [tag{1} tag{3}];
nt = size(t,1);
fclose(tagsFile);

tagsMoviesFile = fopen('TagGenome/tag_relevance.bigdat');
tagmov = textscan(tagsMoviesFile,'%d\t%d\t%f64\n','Delimiter','\t');
mt = [tagmov{1} tagmov{2} tagmov{3}];
mtv = tagmov{3};
%mtv = zscore(mtv);
nmt = size(mt,1);
fclose(tagsMoviesFile);

ids = sortrows(t,2);
ids = ids(:,1);
ids = flipud(ids);
names = tag{2}(ids+1);

nidscor = 5;
corr = zeros(nt,nidscor+1);
corr(:,1) = t(:,1);
curvals = zeros(nidscor,1);

for ii = 1:(nmt/nm)
    ii
    for jj = 1:nidscor
        currFeatId = ids(jj);
        curvals(jj) = mt(ii+currFeatId);

    end
    for kk = 1:nt
        for jj = 1:nidscor
           if(ids(jj) == kk-1)
               continue;
           end
           corr(kk,jj+1) = corr(kk,jj+1)...
               + (curvals(jj) - mtv(ii+kk-1))^2 * double(m(ii,2));
        end
    end
end

%% Write features out to csv file

movieIDs = containers.Map({0},{0});

count = 1;
for ii = 1:size(mt,1)
    ID = mt(ii,1);
    if isKey(movieIDs,ID) == 0 
       movieIDs = [movieIDs; containers.Map({ID},{count})];
       count = count + 1
    end
end

features = zeros(9734,1128);

h = waitbar(0,'Grab a Coffee')

for ii = 1:size(mt,1) 
    movieId = movieIDs(mt(ii,1));
    tagId = mt(ii,2) + 1;
    tagW = mt(ii,3); % tag weight/relevance
    features(movieId,tagId) = tagW;
    waitbar(ii/size(mt,1),h,sprintf('%f',ii/size(mt,1)))
end

clearvars -except features
csvwrite('features.csv',features)

%%

% featnames = tag{2}(ids(1:10)+1);
% feats = rand(nm,10);
% tmp = double(m(:,1));
% feats = [tmp feats];
% 
% outp = fopen('outp.txt','w');
% for ii=1:length(featnames)
%     if(ii < length(featnames))
%         fprintf(outp,'%s~',featnames{ii});
%     else
%         fprintf(outp,'%s\n',featnames{ii});
%     end
% end
% 
% for ii=1:nm
%     fprintf(outp,'%d',feats(ii,1));
%     for jj=2:size(feats,2)
%        fprintf(outp,'~%f',feats(ii,jj)); 
%     end
%     fprintf(outp,'\n');
% end
% fclose(outp);
