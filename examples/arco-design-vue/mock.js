import fetchMock from 'https://cdn.jsdelivr.net/npm/fetch-mock@9.11.0/esm/client.js';
fetchMock.catch(url => {
    if (!url.startsWith('http') && url.startsWith('/')) {
        url = `${document.baseURI.replace(document.location.hash, '')}${url.substr(1)}`;
    }
    return fetchMock.realFetch.bind(window)(url);
});

fetchMock.mock(/.*api\/table.json.*/, url => {
    var search = new URL(`${document.baseURI.replace(document.location.hash, '')}${url}`).search;
    var query = search ? search.substr(1) : null;
    var model = Qs.parse(query);
    model.pageSize = model.pageSize || 20;
    model.current = model.current || 1;
    model.total = 100*model.current;
    return model;
});