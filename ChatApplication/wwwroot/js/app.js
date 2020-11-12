const connection = new signalR.HubConnectionBuilder()
    .withUrl("/Chat")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
            if (retryContext.elapsedMilliseconds < 60000) {
                // If we've been reconnecting for less than 60 seconds so far,
                // wait between 0 and 10 seconds before the next reconnect attempt.
                return Math.random() * 10000;
            } else {
                // If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
                return null;
            }
        },
    })
    .build();


//{
//    id: '',
//        receiverId: '',
//            senderId: '',
//                sendingTime: '',
//                    text: ''
//}

const app = new Vue({
    el: "#app",
    data: {
        connection: null,
        connectionState: {
            connectedState: false,
            reasonMessage: null
        },
        onlineUsers: [],
        currentUser: {
            userId: '',
            userName: '',
            isOnline: false,
            unreadMessageCount: 0,
            messages: []
        },
        authenticatedUser: {
            userId: ''
        },
        authenticated: false,
        messageText: '',
        requestError: '',
        loginData: {
            email: "",
            password: "",
        },
        loginData2: {
            phoneNumber: 0,
        },
        error: {
            email: "",
            password: "",
        },
    },
    created() {
        const token = localStorage.getItem('token');

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/Chat", {
                accessTokenFactory: () => token
            })
            .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: (retryContext) => {
                    if (retryContext.elapsedMilliseconds < 60000) {
                        // If we've been reconnecting for less than 60 seconds so far,
                        // wait between 0 and 10 seconds before the next reconnect attempt.
                        return Math.random() * 10000;
                    } else {
                        // If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
                        return null;
                    }
                },
            })
            .build();
        // closes connection if time since last message received was >= this value
        this.connection.serverTimeoutInMilliseconds = 10 * 10000; // 10 mins

        this.connection.onreconnecting((error) => {
            this.connectionState.connectedState = false;
            this.connectionState.reasonMessage = `connection losed ${error}`;
            console.error(`connection losed ${error}`);
        });

        this.connection.onreconnected((connectionId) => {
            this.connectionState.connectedState = true;
            this.connectionState.reasonMessage = `successfuly connected`;
            console.info(`successfuly connected ${connectionId}`);
        });


        this.connection.on("OnAuthenticate", (reposonse) => {
            this.onAuthenticated(reposonse);
        });
        this.connection.on("OnUsersFeed", (response) => {
            this.OnUsersFeed(response);
        });
        this.connection.on("OnGetUserMessages", (response) => {
            this.OnGetUserMessages(response);
        });
        this.connection.on("OnSendBackMessageToUser", (response) => {
            this.OnSendBackMessageToUser(response);
        });

        this.connectionState = {
            connectedState: this.isConnected,
            reasonMessage: "Disconnected"
        }
        //this.connectionState.connectedState = this.isConnected; //signalR.HubConnectionState.Connected;
        //this.connectionState.reasonMessage = "Disconnected";
        //console.log(this.connectionState); OnSendMessageToUser
    },
    computed: {
        isConnected: function () {
            if (this.connection.state === signalR.HubConnectionState.Disconnected)
                return false;
            else return true;
        },
    },
    methods: {
        async startConnection() {
            try {
                if (this.isConnected === false) {
                    await this.connection.start();
                    // await connection.start();
                    //console.log(this.connection.connection);
                    this.connectionState.connectedState = this.isConnected; //signalR.HubConnectionState.Connected;
                    this.connectionState.reasonMessage = "successfuly connected";
                }
                //console.log(this.connectionState.connectedState);
            } catch (e) {
                this.connectionState.connectedState = this.isConnected; //signalR.HubConnectionState.Connected;
                this.connectionState.reasonMessage = `Can't connect beacuse of ${e.message}`;
                //console.log(e);
            }
        },
        send(hubMethodName, data) {
            try {
                if (this.isConnected === false) {
                    alert("u are ofline");
                    return;
                }
                this.connection.invoke(hubMethodName, data).then((data) => {
                    console.log(data);
                }).catch(error => { this.requestError = error.message; console.error(error) });
            } catch (error) {
                this.requestError = error.message;
                console.error(error.message);
            }
        },
        login() {
            this.send("AuthenticateByEmail", this.loginData);
        },
        login2() {
            this.send("AuthenticateByPhoneNumber", this.loginData2);
        },
        startChatWith(user) {
            this.currentUser = {
                userId: user?.userId,
                userName: user?.userName,
                isOnline: user?.isOnline,
                unreadMessageCount: user?.unreadMessageCount,
            };
            this.messageText = '';
            //Vue.set(this.currentUser, "userId", user?.userId);
            const model = {
                contactUserName: user?.userName,
                contactId: user?.userId,
                pageIndex: 1
            }
            this.send('GetUserMessages', model);//.then(response => console.log('start chat with then', response)).catch(error => console.log('startChat with error', error));
        },
        OnGetUserMessages(response) {
            // Vue.set(this.currentUser, "messages", []);
            if (response?.isSuccessed && response?.data?.some(msg => (msg.receiverId === this.currentUser?.userId && msg.senderId === this.authenticatedUser.userId || msg.receiverId === this.authenticatedUser.userId && msg.senderId === this.currentUser.userId))) {
                // if (response?.isSuccessed && response?.data?.some(msg => msg.receiverId === this.currentUser?.userId)) {// && response?.data?.some(msg => msg.senderId === this.currentUser?.userId || msg.receiverId === this.currentUser?.userId)
                //this.currentUser.messages = response?.data;

                Vue.set(this.currentUser, "messages", response?.data);
                console.log('message inside', response.data);
            } else {
                Vue.set(this.currentUser, "messages", []);
            };
        },
        sendMessage() {
            if (!this.messageText && this.messageText === '')
                alert("Message can't be empty!!")
            else {
                const message = {
                    receiverId: this.currentUser.userId,
                    text: this.messageText,
                    receiverUserName: this.currentUser.userName
                }
                this.send('SendMessageToUser', message);
                console.log('send message', message);
            }
        },
        OnSendBackMessageToUser(response) {
            if (response?.isSuccessed && (response?.data?.receiverId === this.currentUser?.userId && response?.data?.senderId === this.authenticatedUser.userId || response?.data?.receiverId === this.authenticatedUser.userId && response?.data?.senderId === this.currentUser.userId)) {
                this.currentUser.messages.length >= 0 ? this.currentUser.messages.push(response.data) : this.currentUser.messages = messages;//Vue.set(this.currentUser, "messages", response?.data);
                this.messageText = '';
            } else {
                this.requestError = response.message;
                console.log('error sendig message');
            }
        },
        //async testtAuth() { ng ContactUserName
        //    try {           ng ContactId { get
        //        await this.cPageIndex { get; sonnection.invoke('TestAuthentication');
        //    } catch (e) {
        //        console.log(e);
        //    }
        //},
        OnUsersFeed(response) {
            if (response?.isSuccessed) {
                const data = response?.data;
                //console.log('inside success', data);
                this.onlineUsers = data;
            } else {
                this.requestError = reposonse?.message;
            }
        },
        onAuthenticated(responseData) {
            if (responseData.isSuccessed) {
                localStorage.setItem('token', responseData.data.token);
                // this.connection.connection.accessTokenFactory = responseData.data;
                this.authenticated = true;
                this.authenticatedUser.userId = responseData.data.userId;
            } else {
                this.authenticated = false;
            }
        }
    },
});

// connection.onreconnecting((error) => {
//   app.state = false;
//   console.error(`connection losed ${error}`);
// });

// connection.onreconnected((connectionId) => {
//   app.state = true;
//   console.info(`successfuly connected ${connectionId}`);
// });
