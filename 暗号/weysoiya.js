function code() {
    //暗号化
    document.getElementById("output").innerHTML = "";
    var a = document.getElementById("input").value;
    var int = new Array();
    var i = 0, timer = setInterval(function () {
        int[i] = a.slice(i, i + 1);
        if (int[i] == "\n") {
            //改行の処理
            int[i] = "うぇいうぇいうぇいうぇいうぇいうぇいうぇいうぇいうぇいうぇい";
            document.getElementById("output").innerHTML += int[i];
        }
        else {
            int[i] = escape(int[i]);
            if (int[i].indexOf("%u") != -1) {
                int[i] = ("00000000" + (parseInt(int[i].replace("%u", ""), 16).toString(4))).slice(-8);
                int[i] = "45" + int[i];
            }
            else if (int[i].indexOf("%") != -1) {
                int[i] = ("00000000" + (parseInt(int[i].replace("%", ""), 16).toString(4))).slice(-8);
                int[i] = "40" + int[i];
            }
            else {
                int[i] = ("00000000" + (int[i].charCodeAt(0).toString(4))).slice(-8);
                int[i] = "00" + int[i];
            }
            int[i] = int[i]
                .replace(/0/g, "うぇい")
                .replace(/1/g, "そいや")
                .replace(/2/g, "ウェイ")
                .replace(/3/g, "ソイヤ")
                .replace(/4/g, "ｳｪｲ")
                .replace(/5/g, "ｿｲﾔ");
            document.getElementById("output").innerHTML += int[i];
        }
        i++;
        if (i >= a.length) {
            clearInterval(timer);
        }
    }, 0);
}
function decode() {
    //復号化
    document.getElementById("output").innerHTML = "";
    var a = document.getElementById("input").value;
    var int = new Array();
    var i = 0, timer = setInterval(function () {
        int[i] = a.slice(30 * i, 30 * i + 30);
        int[i] = int[i].replace(/うぇい/g, "0")
        .replace(/そいや/g, "1")
        .replace(/ウェイ/g, "2")
        .replace(/ソイヤ/g, "3")
        .replace(/ｳｪｲ/g, "4")
        .replace(/ｿｲﾔ/g, "5");
        if (int[i] == "0000000000") {
            //改行
            int[i] = "<br>";
            document.getElementById("output").innerHTML += int[i];
        }
        else {
            if (int[i].substr(0, 2) == "45") {
                int[i] = parseInt(int[i].substr(2, 8), 4).toString(16);
                int[i] = unescape("%u" + ("0000"+int[i]).slice(-4));
            }
            else if (int[i].substr(0, 2) == "40") {
                int[i] = parseInt(int[i].substr(2, 8), 4).toString(16);
                int[i] = unescape("%" + ("00" + int[i]).slice(-2));
            }
            else if (int[i].substr(0, 2) == "00") {
                int[i] = String.fromCharCode(parseInt(int[i].substr(2, 8), 4));
            }
            document.getElementById("output").innerHTML += int[i];
        }
        i++;
        if (i >= a.length / 30) {
            clearInterval(timer);
        }
    }, 0);
}