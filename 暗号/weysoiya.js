function outp_into_inp() {
    var str = document.getElementById("output").innerHTML;
    str = str.replace(/<br>/g, "\n");
    document.getElementById("input").value = str;
}
function outp_copy() {
    var str = document.getElementById("output").innerHTML;
    str = str.replace(/<br>/g, "\n");
    clipboardData.setData('text', str);

}

function code() {
    //暗号化
    document.getElementById("output").innerHTML = "";
    var a = document.getElementById("input").value;
    var int = new Array();
    var i = 0, timer = setInterval(function () {
        int[i] = a.slice(i, i + 1);
        if (int[i] == "\n") {
            //改行の処理
            int[i] = "うぇいうぇいうぇいうぇいうぇいうぇいうぇいうぇい";
            document.getElementById("output").innerHTML += int[i];
        }
        else {
            int[i] = escape(int[i]);
            if (int[i].indexOf("%u") != -1) {
                //普通のユニコード文字
                int[i] = ("00000000" + (parseInt(int[i].replace("%u", ""), 16).toString(4))).slice(-8);
            }
            else if (int[i].indexOf("%") != -1) {
                //半角カッコとか
                int[i] = ("00000000" + (parseInt(int[i].replace("%", ""), 16).toString(4))).slice(-8);
            }
            else {
                //半角英数字
                int[i] = ("00000000" + (int[i].charCodeAt(0).toString(4))).slice(-8);
            }
            int[i] = int[i]
                .replace(/0/g, "うぇい")
                .replace(/1/g, "そいや")
                .replace(/2/g, "ウェイ")
                .replace(/3/g, "ソイヤ");
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
        int[i] = a.slice(24 * i, 24 * i + 24);
        int[i] = int[i].replace(/うぇい/g, "0")
        .replace(/そいや/g, "1")
        .replace(/ウェイ/g, "2")
        .replace(/ソイヤ/g, "3");
        if (int[i] == "00000000") {
            //改行
            int[i] = "<br>";
            document.getElementById("output").innerHTML += int[i];
        }
        else {
            int[i] = parseInt(int[i], 4).toString(16);
            int[i] = unescape("%u" + ("0000" + int[i]).slice(-4));
            document.getElementById("output").innerHTML += int[i];
        }
        i++;
        if (i >= a.length / 24) {
            clearInterval(timer);
        }
    }, 0);
}