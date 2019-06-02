GeoPlugin = (function () {
    function GeoPlugin(option) {
        var that = this;

        this.selectors = {
            inputs: {
                ip: "#ip",
                city: "#city"
            },
            forms: {
                ip: "#ip-form",
                city: "#city-form"
            },
            pills: {
                ip: "#v-pills-ip",
                city: "#v-pills-city"
            },
            tableTemplate: "#table_tmp",
            perfomanceBlock: ".performance-block"
        }
        
        this.HELPER_CLASSES = {
            HAS_ERROR: "has-error"
        }

        /**
         * Метод обработки результатов. Выводит результаты на страницу
         * @param {any} result Полученный json объект
         * @param {Object<>} pill Селектор соответствущей вкладки
         */
        function onResultReceived(result, pill)
        {
            var tableBody = window.$("table>tbody", pill);
            if (!result || result.length === 0) {
                tableBody.empty().text("Нет результатов");
            } else {
                var templateHtml = "";
                var tableTemplate = window.$(that.selectors.tableTemplate).html();
                if (Array.isArray(result)) {
                    for (var i = 0; i < result.length; i++) {
                        templateHtml += window.Mustache.to_html(tableTemplate, result[i]);
                    }
                } else {
                    templateHtml = window.Mustache.to_html(tableTemplate, result);
                }
                tableBody.empty().append(templateHtml);
            }
        }

        /**
         * Метод инициализации плагина
         */
        GeoPlugin.prototype.init = function () {
            var that = this;
            that.registerObservers();
        }

        /**
         * Метод подписки на события
         */
        GeoPlugin.prototype.registerObservers = function () {
            var that = this;

            $(that.selectors.forms.ip).submit(function (e) {
                e.preventDefault();
                
                that.getLocationByIp($(that.selectors.inputs.ip).val(),
                    function (result, status, xhr) {
                        onResultReceived(result, that.selectors.pills.ip);
                    });
            });

            $(that.selectors.forms.city).submit(function (e) {
                e.preventDefault();

                that.getLocationByCity($(that.selectors.inputs.city).val(),
                    function (result, status, xhr) {
                        onResultReceived(result, that.selectors.pills.city);
                    });
            });
        }

        /**
         * Метод получения местоположения по IP-адресу
         * @param {string} ip IP-адрес
         * @param {function()} successCallback Функция обратного вызова при успешном результате запроса
         * @param {function()} errorCallback Функция обратного вызова при ошибочном результате запроса
         */
        GeoPlugin.prototype.getLocationByIp = function(ip, successCallback, errorCallback) {
            $.ajax({
                url: "/api/ip/" + ip + "/location",
                type: 'GET',
                dataType: 'json',
                success: successCallback,
                error: errorCallback
            });
        }

        /**
         * Метод получения местоположения по названию города
         * @param {string} city Название города
         * @param {function()} successCallback Функция обратного вызова при успешном результате запроса
         * @param {function()} errorCallback Функция обратного вызова при ошибочном результате запроса
         */
        GeoPlugin.prototype.getLocationByCity = function (city, successCallback, errorCallback) {
            $.ajax({
                url: "/api/city/" + city + "/locations",
                type: 'GET',
                dataType: 'json',
                success: successCallback,
                error: errorCallback
            });
        }
    }

    return GeoPlugin;
})();