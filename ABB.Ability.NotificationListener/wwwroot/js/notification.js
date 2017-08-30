$(function () {
    var connection = $.hubConnection("http://" + signalRHost);
    var abilityNotificationHubProxy = connection.createHubProxy('abilityNotificationHub');
    abilityNotificationHubProxy.on('showNotificationInPage', function (message) {
        $("#notificationArea").append(message).append("<br/>");
    });
    connection.start().done(function () {
        // send notification to the server.
    });

});