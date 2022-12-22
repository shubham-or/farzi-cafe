mergeInto(LibraryManager.library, {  
	Web3LoginRequest: function (_objectName, _methodName) {    
		window.dispatchReactUnityEvent("Unity_Web3Login", UTF8ToString(_objectName), UTF8ToString(_methodName));  
	},

	Web3LogoutRequest: function (_objectName, _methodName) {    
		window.dispatchReactUnityEvent("Unity_Web3Logout", UTF8ToString(_objectName), UTF8ToString(_methodName));  
	},

	QuitGame: function (){
        window.dispatchReactUnityEvent("Unity_QuitGame");  
    }
});