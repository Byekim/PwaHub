window.indexedDBFunctions = {
    openDatabase: function () {
        let request = indexedDB.open('AppDB', 1);
        request.onupgradeneeded = function (event) {
            let db = event.target.result;
            if (!db.objectStoreNames.contains('apiData')) {
                db.createObjectStore('apiData', { keyPath: 'id' });  // id를 키로 사용
            }
        };
        request.onsuccess = function (event) {
            console.log("Database opened successfully.");
        };
        request.onerror = function (event) {
            console.error("Error opening database.", event);
        };
    },

    saveData: function (id, data) {
        return new Promise((resolve, reject) => {
            let request = indexedDB.open('AppDB', 1);
            request.onsuccess = function (event) {
                let db = event.target.result;
                let transaction = db.transaction('apiData', 'readwrite');
                let store = transaction.objectStore('apiData');

                let dataRecord = {
                    id: id,  // 키 값을 명시적으로 지정
                    content: data
                };

                let addRequest = store.put(dataRecord);  // put()을 사용하면 기존 키가 있으면 덮어씀
                addRequest.onsuccess = function () {
                    resolve("Data saved successfully");
                };
                addRequest.onerror = function (event) {
                    reject("Error saving data.");
                };
            };
            request.onerror = function (event) {
                reject("Error opening database.");
            };
        });
    },

    getAllData: function () {
        return new Promise((resolve, reject) => {
            let request = indexedDB.open('AppDB', 1);
            request.onsuccess = function (event) {
                let db = event.target.result;
                let transaction = db.transaction('apiData', 'readonly');
                let store = transaction.objectStore('apiData');
                let getAllRequest = store.getAll();

                getAllRequest.onsuccess = function () {
                    resolve(getAllRequest.result);
                };
                getAllRequest.onerror = function () {
                    reject("Error retrieving data.");
                };
            };
            request.onerror = function () {
                reject("Error opening database.");
            };
        });
    }
};
