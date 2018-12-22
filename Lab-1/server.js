var net = require('net');
var streams = Array();
var str;
var strPrs;

function onStreamData(data) {
    streams.forEach(function (stream) {
        if (0 === data.indexOf('echo')) {
            stream.write(str, 'utf8');
        } else if (0 === data.indexOf('close') || 0 === data.indexOf('exit')) {
            console.log(data);
            server.close();
            process.exit(0);
        }
        else {
            str = data;
            strPrs = str.split('_');
            strPrs.reverse();
            stream.write(strPrs.join('_'), 'utf8');
        }     
    });
}

var server = net.createServer(function (c) {
    c.setEncoding('utf8');
    streams.push(c);
    streams.forEach(function (stream) {
        stream.write(c.remoteAddress + ' - is connected\n', 'utf8');
    });
    c.on('data', onStreamData);
});

server.on('close', function () {
    streams.forEach(function (stream) {
        stream.write('Server is going down!', 'utf8');
        stream.destroy();
    });
});

server.maxConnections = 2; 
server.listen
