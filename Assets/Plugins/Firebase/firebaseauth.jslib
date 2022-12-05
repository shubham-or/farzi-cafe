mergeInto(LibraryManager.library, {

    CreateUserWithEmailAndPassword: function (email, password, objectName, onSuccess, onFailed) {
        var parsedEmail = UTF8ToString(email);
        var parsedPassword = UTF8ToString(password);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            firebase.auth().createUserWithEmailAndPassword(parsedEmail, parsedPassword).then(function (result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: signed up for " + parsedEmail);
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithEmailAndPassword: function (email, password, objectName, onSuccess, onFailed) {
        var parsedEmail = UTF8ToString(email);
        var parsedPassword = UTF8ToString(password);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {

            firebase.auth().signInWithEmailAndPassword(parsedEmail, parsedPassword).then(function (result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, "Success: signed in for " + parsedEmail);
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithGoogle: function (objectName, onSuccess, onFailed) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            var provider = new firebase.auth.GoogleAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(result));
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithFacebook: function (objectName, onSuccess, onFailed) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            var provider = new firebase.auth.FacebookAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (unused) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(result));
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignOut: function (objectName, onSuccess, onFailed) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        try {
            firebase.auth().signOut().then(function (result) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(result));
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    OnAuthStateChanged: function (objectName, onUserSignedIn, onUserSignedOut) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnUserSignedIn = UTF8ToString(onUserSignedIn);
        var parsedOnUserSignedOut = UTF8ToString(onUserSignedOut);

        firebase.auth().onAuthStateChanged(function(user) {
            if (user) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnUserSignedIn, JSON.stringify(user));
            } else {
                unityInstance.Module.SendMessage(parsedObjectName, parsedOnUserSignedOut, "User signed out");
            }
        });
    },

     GetUser: function (objectName, onSuccess, onFailed) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnSuccess = UTF8ToString(onSuccess);
        var parsedOnFailed = UTF8ToString(onFailed);

        var currentUser = firebase.auth().currentUser;
        if (currentUser) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnSuccess, JSON.stringify(JSON.parse(currentUser).uid));
        } else {
            unityInstance.Module.SendMessage(parsedObjectName, parsedOnFailed, "No user found!");
        }
    }
});
