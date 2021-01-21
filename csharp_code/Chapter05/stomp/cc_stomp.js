var stompClient;

var stompConfig = {
    connectHeaders: {
        login: "cc-dev",
        passcode: "taxi123",
        host: "cc-dev-ws"
    },
    brokerURL: "ws://localhost:15674/ws",
    debug: function (str) {
        console.log('STOMP: ' + str);
    },
    reconnectDelay: 200,
    onConnect: function (frame) {
        var subscription = stompClient.subscribe('/queue/taxi_information', function (message) {
            var body = JSON.parse(message.body);
            var latitude = body.latitude;
            var longitude = body.longitude;
            console.log(body);
            var bodyJson = JSON.stringify(body);
            document.getElementById("last-message").innerHTML = bodyJson;
        });
    }
};
stompClient = new StompJs.Client(stompConfig);
stompClient.activate();