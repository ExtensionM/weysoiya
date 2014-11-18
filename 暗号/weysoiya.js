var ws = new WeySoiya();
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
    //暗号化
    document.getElementById("output").innerHTML = "";
    ws.code(document.getElementById("input").value, document.getElementById("type").selectedIndex);
}
function decode() {
    //復号化
    document.getElementById("output").innerHTML = "";
    ws.decode(document.getElementById("input").value, document.getElementById("type").selectedIndex);

}