const logger = function (req, res, next ){
    console.log('Nello');
    next();
};

module.exports = logger;