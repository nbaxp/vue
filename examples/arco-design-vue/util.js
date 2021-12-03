function LocalStorageGetOrAddItem(key, value) {
    let item = localStorage.getItem(key);
    if (!item) {
        localStorage.setItem(key, value);
        item = localStorage.getItem(key);
    }
    return item;
}

function LocalStorageUpdateItem(key, value) {
    localStorage.setItem(key, value);
    let item = localStorage.getItem(key);
    return item;
}
