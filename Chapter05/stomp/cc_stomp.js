let stompClient;

const stompConfig = {
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
        const subscription =stompClient.subscribe('/queue/taxi_information', function (message) {
            const body = JSON.parse(message.body);
            const latitude = body.latitude;
            const longitude = body.longitude;
            console.log(body);
        });
    }};
stompClient = new StompJs.Client(stompConfig);
stompClient.activate();