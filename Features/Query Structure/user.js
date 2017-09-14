OnApplicationReady(function() {
    QB.Web.Core.bind(QB.Web.Core.Events.UserDataReceived, onUserDataReceived);
});


onUserDataReceived = function(e, data)
{
    $('#alternate-sql textarea').val(data.AlternateSQL);
};