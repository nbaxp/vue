import fetchMock from 'https://cdn.jsdelivr.net/npm/fetch-mock@9.11.0/esm/client.js';

fetchMock.catch(url => {
    if (!url.startsWith('http') && url.startsWith('/')) {
        url = `${document.baseURI.replace(document.location.hash, '')}${url.substr(1)}`;
    }
    return fetchMock.realFetch.bind(window)(url);
});

fetchMock.mock(/.*api\/table.json.*/, url => {
    var search = new URL(`${document.baseURI.replace(document.location.hash, '')}${url}`).search;
    var query = search ? search.substring(1) : null;
    var model = Qs.parse(query);
    model.pageSize = parseInt(model.pageSize || 10);
    model.pageIndex = parseInt(model.pageIndex || 1);
    model.total = 999;
    return model;
});