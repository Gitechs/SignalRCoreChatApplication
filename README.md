# SignalRCoreChatApplication
In this repository, we're using [SignalRCore](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-3.1)
## Some features we are using, are : 
- handling auto hub connecting
- JWT Authentication
- Client Vuejs App
- track online users
- send a text to users
- use EF Core (in-memory Database)

# How does it works?

after restoring client-side and server-side packages. run the application, and first connect to the hub by clicking on a button at top of the page then login with username and password
or just log in with mobile number. if you are login successfully then the login form disappears and the given Jw token will be saved in local storage.
now you have to refresh the page and the hub will be connected and all users will be loaded.

 :beginner:  Please give a star  :star:
