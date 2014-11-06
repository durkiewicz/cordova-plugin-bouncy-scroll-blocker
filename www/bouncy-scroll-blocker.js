var BouncyScrollBlocker = function () {};

BouncyScrollBlocker.prototype = {
    block: function (options) {
        cordova.exec(null, null, 'BouncyScrollBlocker', 'block', []);
    }
};

var plugin = new BouncyScrollBlocker();

module.exports = plugin;