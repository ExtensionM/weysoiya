var aa = function () {
    this.str = new Array();
    this.str[2] = new Array("うぇい", "そいや", "ウェイ", "ソイヤ");
    this.str[4] = new Array("ウェイ", "ソイヤ", "うぇい", "そいや", "ウェい", "ソイや", "ウぇイ", "ソぃヤ", "ウぇい", "ソいや", "うェイ", "そイヤ", "うぇイ", "そいヤ", "うェい", "そイや");
    this.RE = new Array();
    this.RE[2] = new Array();
    for (var i = 0; i < 4; i++) {
        this.RE[2][i] = new RegExp(this.str[2][i], "g");
    }
    this.RE[4] = new Array();
    for (var i = 0; i < 16; i++) {
        this.RE[4][i] = new RegExp(this.str[4][i], "g");
    }
    this.code = function (string, num) {
        var int = new Array();
        var i = 0, timer = setInterval(function () {
            int[i] = string.slice(i, i + 1);
            if (int[i] == "\n") {
                //改行の処理
                int[i] = "";
                mypow("うぇい", num);
                document.getElementById("output").innerHTML += int[i];
            }
            else {
                int[i] = escape(int[i]);
                if (int[i].indexOf("%u") != -1) {
                    //普通のユニコード文字
                    int[i] = (mypow("0", num) + (parseInt(int[i].replace("%u", ""), 16).toString(Math.pow(2, num)))).slice(-16/num);
                }
                else if (int[i].indexOf("%") != -1) {
                    //半角カッコとか
                    int[i] = (mypow("0", num) + (parseInt(int[i].replace("%", ""), 16).toString(Math.pow(2, num)))).slice(-16 / num);
                }
                else {
                    //半角英数字
                    int[i] = (mypow("0", num) + (int[i].charCodeAt(0).toString(Math.pow(2,num)))).slice(-16/num);
                }
                int[i] = int[i]
                    .replace(/0/g, "うぇい")
                    .replace(/1/g, "そいや")
                    .replace(/2/g, "ウェイ")
                    .replace(/3/g, "ソイヤ");
                document.getElementById("output").innerHTML += int[i];
            }
            i++;
            if (i >= string.length) {
                document.getElementById("outp_length").innerHTML =
                    document.getElementById("output").innerHTML.length;
                clearInterval(timer);
            }
        }, 0);
    }
    this.decode = function (string, num) {
        var int = new Array();
        var i = 0, timer = setInterval(function () {
            int[i] = string.slice(24 * i, 24 * i + 24);
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
            if (i >= string.length / 24) {
                document.getElementById("outp_length").innerHTML =
                    document.getElementById("output").innerHTML.length;
                clearInterval(timer);
            }
        }, 0);
    }
}
var ws = new aa();
function mypow(string,num) {
    var result;
    for (var j = 0; j < 16 / num ; j++) {
        result += string;
    }
    return result;
}
function outp_into_inp() {
    var str = document.getElementById("output").innerHTML;
    str = str.replace(/<br>/g, "\n");
    document.getElementById("input").value = str;
}
function outp_copy() {
    var str = document.getElementById("output").innerHTML;
    str = str.replace(/<br>/g, "\n");
    window.clipboardData.setData('text', str);
}

function code() {
    //alert(document.getElementById("type").selectedIndex);
    //document.getElementById("type").options[2].text = "aaaa";
    //暗号化
    document.getElementById("output").innerHTML = "";
    ws.code(document.getElementById("input").value, 2);
}
function decode() {
    //復号化
    document.getElementById("output").innerHTML = "";
    ws.decode(document.getElementById("input").value, 2);

}