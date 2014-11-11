function code() {
    //暗号化
    document.getElementById("output").innerHTML = "";
    var a = document.getElementById("input").innerHTML;
    var int = new Array();
    for (var i = 0; i < a.length; i++) {
        int[i] = a.slice(i, i + 1);
        int[i] = escape(int[i]).replace("%u", "");
        int[i] = ("00000000"+(parseInt(int[i], 16).toString(4))).slice(-8);
        int[i] = int[i]
            .replace(/0/g, "うぇい")
            .replace(/1/g, "そいや")
            .replace(/2/g, "ウェイ")
            .replace(/3/g, "ソイヤ");
        document.getElementById("output").innerHTML += int[i];
    }
}
function decode() {
    //復号化
    document.getElementById("output").innerHTML = "";
    var a = document.getElementById("input").innerHTML;
    var aa = a
        .replace(/うぇい/g, "0")
        .replace(/そいや/g, "1")
        .replace(/ウェイ/g, "2")
        .replace(/ソイヤ/g, "3");
    var int = new Array();
    for (var i = 0; i < a.length/24; i++) {
        int[i] = aa.slice(8 * i, 8 * i + 8);
        int[i] = parseInt(int[i], 4).toString(16);
        int[i] = unescape("%u"+int[i]);
        document.getElementById("output").innerHTML += int[i];
    }
}