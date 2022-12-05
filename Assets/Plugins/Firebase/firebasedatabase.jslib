mergeInto(LibraryManager.library, {

    GetJSON: function(path, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).once('value').then(function(snapshot) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PostJSON: function(path, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).set(JSON.parse(parsedValue)).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PushJSON: function(path, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).push().set(JSON.parse(parsedValue)).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedValue + " was pushed to " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    UpdateJSON: function(path, key, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedKey = UTF8ToString(key);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).update({ parsedKey : parsedValue }).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedValue + " was updated in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    UpdateUserName: function(path, key, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedKey = UTF8ToString(key);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).update({ userName : parsedValue }).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedValue + " was updated in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    UpdateScore: function(path, key, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedKey = UTF8ToString(key);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).update({ score : parsedValue }).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedValue + " was updated in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    UpdateRoomID: function(path, key, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedKey = UTF8ToString(key);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).update({ roomId : parsedValue }).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedValue + " was updated in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    UpdateRoomName: function(path, key, value, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedKey = UTF8ToString(key);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).update({ roomName : parsedValue }).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedValue + " was updated in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    DeleteJSON: function(path, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).remove().then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: " + parsedPath + " was deleted");
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForValueChanged: function(path, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).on('value', function(snapshot) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForValueChanged: function(path, parsedObjectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).off('value');
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: listener removed");
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForChildAdded: function(path, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).on('child_added', function(snapshot) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForChildAdded: function(path, parsedObjectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).off('child_added');
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: listener removed");
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForChildChanged: function(path, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).on('child_changed', function(snapshot) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForChildChanged: function(path, parsedObjectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).off('child_changed');
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: listener removed");
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForChildRemoved: function(path, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).on('child_removed', function(snapshot) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForChildRemoved: function(path, parsedObjectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).off('child_removed');
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: listener removed");
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ModifyNumberWithTransaction: function(path, amount, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).transaction(function(currentValue) {
                if (!isNaN(currentValue)) {
                    return currentValue + amount;
                } else {
                    return amount;
                }
            }).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: transaction run in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ToggleBooleanWithTransaction: function(path, objectName, onSuccess, onFailed) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.database().ref(parsedPath).transaction(function(currentValue) {
                if (typeof currentValue === "boolean") {
                    return !currentValue;
                } else {
                    return true;
                }
            }).then(function(result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: transaction run in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    }

});