﻿function PrintElement(elem) {
    var screenSize = {
        height: window.screen.height,
        width: window.screen.width
    }
    var mywindow = window.open('', 'PRINT', `height=${screen.height},width=${screenSize.width - 100}`);

    mywindow.document.write('<html><head><title>' + document.title + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h1>' + document.title + '</h1>');
    mywindow.document.write(elem.innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}