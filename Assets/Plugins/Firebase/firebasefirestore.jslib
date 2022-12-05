mergeInto(LibraryManager.library, {

    GetDocument: function (collectionPath, documentId, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.firestore().collection(parsedPath).doc(parsedId).get().then(function (doc) {

                if (doc.exists) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(doc.data()));
                } else {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "null");
                }
            }).catch(function(error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    GetDocumentsInCollection: function (collectionPath, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.firestore().collection(parsedPath).get().then(function (querySnapshot) {

                var docs = {};
                querySnapshot.forEach(function(doc) {
                    docs[doc.id] = doc.data();
                });

                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(docs));
            }).catch(function(error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SetDocument: function (collectionPath, documentId, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            firebase.firestore().collection(parsedPath).doc(parsedId).set(JSON.parse(parsedValue)).then(function() {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: document " + parsedId + " was set");
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    AddDocument: function (collectionPath, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            firebase.firestore().collection(parsedPath).add(JSON.parse(parsedValue)).then(function(unused) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: document added in collection " + parsedPath);
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    UpdateDocument: function (collectionPath, documentId, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            firebase.firestore().collection(parsedPath).doc(parsedId).update(JSON.parse(parsedValue)).then(function() {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: document " + parsedId + " was updated");
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    DeleteDocument: function (collectionPath, documentId, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            firebase.firestore().collection(parsedPath).doc(parsedId).delete().then(function() {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: document " + parsedId + " was deleted");
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    DeleteField: function (collectionPath, documentId, field, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedField = UTF8ToString(field);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            var value = {};
            value[parsedField] = firebase.firestore.FieldValue.delete();

            firebase.firestore().collection(parsedPath).doc(parsedId).update(value).then(function() {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: field " + parsedField + " was deleted");
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    AddElementInArrayField: function (collectionPath, documentId, field, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedField = UTF8ToString(field);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            var value = {};
            value[parsedField] = firebase.firestore.FieldValue.arrayUnion(JSON.parse(parsedValue));

            firebase.firestore().collection(parsedPath).doc(parsedId).update(value).then(function() {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: element " + parsedValue + " was added in " + parsedField);
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    RemoveElementInArrayField: function (collectionPath, documentId, field, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedField = UTF8ToString(field);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            var value = {};
            value[parsedField] = firebase.firestore.FieldValue.arrayRemove(JSON.parse(parsedValue));

            firebase.firestore().collection(parsedPath).doc(parsedId).update(value).then(function() {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: element " + parsedValue + " was removed in " + parsedField);
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    IncrementFieldValue: function (collectionPath, documentId, field, increment, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedField = UTF8ToString(field);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            var value = {};
            value[parsedField] = firebase.firestore.FieldValue.increment(increment);

            firebase.firestore().collection(parsedPath).doc(parsedId).update(value).then(function() {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: incremented " + parsedField + " by " + increment);
            })
                .catch(function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForDocumentChange: function (collectionPath, documentId, includeMetadataChanges, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            if (typeof firestorelisteners === 'undefined') firestorelisteners = {};

            this.firestorelisteners[parsedPath + "/" + parsedId] = firebase.firestore().collection(parsedPath).doc(parsedId)
                .onSnapshot({
                    includeMetadataChanges: (includeMetadataChanges == 1)
                }, function(doc) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(doc.data()));
                }, function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForDocumentChange: function (collectionPath, documentId, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedId = UTF8ToString(documentId);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            if (typeof firestorelisteners === 'undefined') firestorelisteners = {};

            this.firestorelisteners[parsedPath + "/" + parsedId]();
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: listener was removed");
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForCollectionChange: function (collectionPath, includeMetadataChanges, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            if (typeof firestorelisteners === 'undefined') firestorelisteners = {};

            this.firestorelisteners[parsedPath + "/collection/"] = firebase.firestore().collection(parsedPath)
                .onSnapshot({
                    includeMetadataChanges: (includeMetadataChanges == 1)
                }, function(querySnapshot) {

                    var docs = {};
                    querySnapshot.forEach(function(doc) {
                        docs[doc.id] = doc.data();
                    });

                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(docs));

                }, function(error) {
                    unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForCollectionChange: function (collectionPath, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(collectionPath);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            if (typeof firestorelisteners === 'undefined') firestorelisteners = {};

            this.firestorelisteners[parsedPath + "/collection/"]();
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: listener was removed");
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    }

});