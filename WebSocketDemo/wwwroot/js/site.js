function setupWebSocket(targetElement) {
    const socket = new WebSocket("wss://" + window.location.host + "/ws");

    socket.onopen = function (event) {
        console.log("WebSocket opened.");
    };

    socket.onmessage = function (event) {
        console.log("WebSocket message received:", event.data);
        targetElement.textContent = event.data;
    };

    socket.onclose = function (event) {
        console.log("WebSocket closed.");
    };
}