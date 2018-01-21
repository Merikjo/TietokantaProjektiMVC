/// <reference path="../typings/jquery/jquery.d.ts" />
var tuntiModel = /** @class */ (function () {
    function tuntiModel() {
    }
    return tuntiModel;
}());
function initHenkiloTunti() {
    $("#projektiButton").click(function () {
        //alert("Toimii!");
        var projektiTunnit = $("#ProjektiTunnit").val();
        var projektiId = $("#ProjektiID").val();
        var henkiloId = $("#HenkiloID").val();
        //alert("L: " + locationCode + ", A:" + assetCode);
        //määritetään muuttuja:
        var data = new tuntiModel();
        data.ProjektiTunnit = projektiTunnit;
        data.ProjektiID = projektiId;
        data.HenkiloID = henkiloId;
        //lähetetään JSON-muotoista dataa palvelimelle
        $.ajax({
            type: "POST",
            url: "/Henkilo/GetHenkiloTunnit",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (data) {
                if (data.success === true) {
                    alert("Asset successfully assigned.");
                }
                else {
                    alert("There was an error: " + data.error);
                }
            },
            dataType: "json"
        });
    });
}
//# sourceMappingURL=Logic.js.map