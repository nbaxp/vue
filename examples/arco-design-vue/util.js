function GetOrAddLocalStorageItem(key, value) {
    let item = localStorage.getItem(key);
    if (!item) {
        localStorage.setItem(key, value);
        item = localStorage.getItem(key);
    }
    return item;
}

function UpdateLocalStorageItem(key, value) {
    localStorage.setItem(key, value);
    return localStorage.getItem(key);
}
