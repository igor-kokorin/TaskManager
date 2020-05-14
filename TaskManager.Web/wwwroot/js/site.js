$(function () {
    function setTaskTree() {
        $("#task-tree .item").mouseenter(function () {
            $(this).find(".icon").first().removeClass("disabled");
        });

        $("#task-tree .item").mouseleave(function () {
            $(this).find(".icon").first().addClass("disabled");
        });

        $("#task-tree .item .header").click(function (e) {
            e.preventDefault();

            $.get(this.href).then(function (data) {
                $("#taskDisplay").html(data);
            });
        });
    }

    function setTaskLink() {
        $("#taskDisplay").bind("DOMSubtreeModified", function () {
            $(".task-link").click(function (e) {
                e.preventDefault();

                $.get(this.href).then(function (data) {
                    $("#taskDisplay").html(data);
                });
            });
        });
    }

    function setStatusToggle() {
        $("form .status-toggle button").click(function () {
            $("form .status-toggle button").removeClass("active");

            $(this).addClass("active");

            $("form #Status").val($(this).data("status"));
        });
    }

    function setFormValidation() {
        function isTimeSpanComponent(param, upperLimit) {
            param = Number(param);

            return !Number.isNaN(param) && param >= 0 && (upperLimit === undefined || param < upperLimit);
        }

        $.fn.form.settings.rules.executionTime = function (param) {
            const parts = param.split(':');

            if (parts.length !== 3) {
                return false;
            }

            const [daysAndHours, minutes, seconds] = parts;

            let [days, hours] = daysAndHours.split('.');

            if (hours === undefined) {
                hours = days;
                days = undefined;
            }

            return (days === undefined || isTimeSpanComponent(days)) && isTimeSpanComponent(hours, 24) && isTimeSpanComponent(minutes, 60) && isTimeSpanComponent(seconds, 60);
        }

        $('.ui.form')
            .form({
                inline: true,
                fields: {
                    Title: {
                        rules: [
                            {
                                type: 'empty',
                                prompt: 'Обязательное поле'
                            },
                            {
                                type: 'maxLength[100]',
                                prompt: 'Длина не должна превышать 100 символов'
                            }
                        ]
                    },
                    Description: {
                        rules: [
                            {
                                type: 'empty',
                                prompt: 'Обязательное поле'
                            },
                            {
                                type: 'maxLength[1000]',
                                prompt: 'Длина не должна превышать 1000 символов'
                            }
                        ]
                    },
                    Executors: {
                        rules: [
                            {
                                type: 'empty',
                                prompt: 'Обязательное поле'
                            },
                            {
                                type: 'maxLength[100]',
                                prompt: 'Длина не должна превышать 200 символов'
                            }
                        ]
                    },
                    PlannedExecutionTime: {
                        rules: [
                            {
                                type: 'empty',
                                prompt: 'Обязательное поле'
                            },
                            {
                                type: 'executionTime',
                                prompt: 'Вы должны указать время в формате дд.чч:мм:сс'
                            }
                        ]
                    },
                    ActualExecutionTime: {
                        rules: [
                            {
                                type: 'empty',
                                prompt: 'Обязательное поле'
                            },
                            {
                                type: 'executionTime',
                                prompt: 'Вы должны указать время в формате дд.чч:мм:сс'
                            }
                        ]
                    },
                }
            });
    }

    setTaskTree();

    setFormValidation();

    setStatusToggle();

    setTaskLink();
});
