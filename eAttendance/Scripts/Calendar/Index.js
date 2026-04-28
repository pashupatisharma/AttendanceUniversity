$(document).ready(function () {
    //highlight selected field

    $('td').on('mouseover', function () {

        if ($(this).css("background-color") == "rgb(255, 255, 255)") {
            $(this).css("background-color", "#FFDFCC");
        }
    });

    $('td').on('mouseleave', function () {

        if ($(this).css("background-color") == "rgb(255, 223, 204)") {
            $(this).css("background-color", "rgb(255, 255, 255)");
        }
    });


    $(".month").on("click", function () {
        var object = $(this).attr("id");
        var str = object.split('/');
        var today = '';
        // str[0] contains "month"
        // str[1] contains "year"
        $.ajax
        ({
            url: '../../Utility/AsyncTodayDateBS',
            type: 'GET',
            traditional: true,
            async: false,
            contentType: 'application/json',
            data: {},
            success: function (result) {
                today = result;
            }
        });

        $.ajax
        ({
            url: '../../HolidayCalendar/AsyncUpdateCalender',
            type: 'GET',
            traditional: true,
            contentType: 'application/json',
            data: { month: str[0], year: str[1], officeId: str[2]},
            success: function (result) {

                if (!jQuery.isEmptyObject(result)) {
                    var week1 = $("#week1");
                    week1.empty();
                    var week2 = $("#week2");
                    week2.empty();
                    var week3 = $("#week3");
                    week3.empty();
                    var week4 = $("#week4");
                    week4.empty();
                    var week5 = $("#week5");
                    week5.empty();
                    var week6 = $("#week6");
                    week6.empty();
                    $('.fc-prev-button').attr('id', result.prevMonth);
                    $('.fc-next-button').attr('id', result.nextMonth);
                    $('.fc-date-button h2').text(getMonth(str[0]) + ' ' + str[1]);

                    $.each(result.Week1, function (i, item) {
                        var htmlStr = null;
                        if (jQuery.isEmptyObject(item)) {
                            htmlStr = $('<td></td>');
                            week1.append(htmlStr);
                        } else {
                            if (item.daycolumn == 0 || item.daycolumn == 6) {                               
                                if (item.dateStr == today) {
                                    htmlStr = $('<td class="weekend selected"></td>');
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');

                                        });
                                    }
                                    htmlStr.append('</a>');
                                }
                                else {
                                    htmlStr = $('<td class="weekend"></td>');
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                }
                            }
                            else if (item.dateStr != today) {
                                htmlStr = $('<td></td>');
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                            } else {
                                htmlStr = $('<td class="selected"></td>');
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                            }

                            week1.append(htmlStr);

                        }

                    });
                    $.each(result.Week2, function (i, item) {
                        var htmlStr = null;
                        if (jQuery.isEmptyObject(item)) {
                            htmlStr = $('<td></td>');
                            week2.append(htmlStr);
                        } else {

                            if (item.daycolumn == 0 || item.daycolumn == 6) {
                                if (item.dateStr == today) {

                                    htmlStr = $('<td class="weekend selected"></td>');
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');


                                    //htmlStr = $('<td class="weekend selected"></td>');
                                    //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');

                                }
                                else {
                                    htmlStr = $('<td class="weekend"></td>');
                                   
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                    //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                                }
                            }
                            else if (item.dateStr != today) {
                                htmlStr = $('<td></td>');
                               
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                                //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3><span class="label label-primary">' + item.holidayname + '</span> </a>');
                            } else {
                                htmlStr = $('<td class="selected"></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                                //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                            }
                            week2.append(htmlStr);
                        }
                    });
                    $.each(result.Week3, function (i, item) {
                        var htmlStr = null;
                        if (jQuery.isEmptyObject(item)) {
                            htmlStr = $('<td></td>');
                            week3.append(htmlStr);
                        } else {
                            if (item.daycolumn == 0 || item.daycolumn == 6) {
                                if (item.dateStr == today) {
                                   
                                    htmlStr = $('<td class="weekend selected"></td>');
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                    //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                                }
                                else {
                                    htmlStr = $('<td class="weekend"></td>');
                                   
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                    //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                                }
                            }
                            else if (item.dateStr != today) {
                                htmlStr = $('<td></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                                //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                            } else {
                                htmlStr = $('<td class="selected"></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                                //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3><span class="label label-primary">' + item.holidayname + '</span> </a>');
                            }
                            week3.append(htmlStr);
                        }
                    });
                    $.each(result.Week4, function (i, item) {
                        var htmlStr = null;
                        if (jQuery.isEmptyObject(item)) {
                            htmlStr = $('<td></td>');
                            week4.append(htmlStr);
                        } else {
                            if (item.daycolumn == 0 || item.daycolumn == 6) {
                                if (item.dateStr == today) {
                                    htmlStr = $('<td class="weekend selected"></td>');
                                   
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                    //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                                }
                                else {
                                    htmlStr = $('<td class="weekend"></td>');
                                   
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                    //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                                }
                            }
                            else if (item.dateStr != today) {
                                htmlStr = $('<td></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                                //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3><span class="label label-primary">' + item.holidayname + '</span> </a>');
                            } else {
                                htmlStr = $('<td class="selected"></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                                //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3><span class="label label-primary">' + item.holidayname + '</span> </a>');
                            }
                            week4.append(htmlStr);
                        }
                    });
                    $.each(result.Week5, function (i, item) {
                        var htmlStr = null;
                        if (jQuery.isEmptyObject(item)) {
                            htmlStr = $('<td></td>');
                            week5.append(htmlStr);
                        } else {
                            if (item.daycolumn == 0 || item.daycolumn == 6) {
                                if (item.dateStr == today) {
                                    htmlStr = $('<td class="weekend selected"></td>');
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                    //htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3> <span class="label label-primary">' + item.holidayname + '</span></a>');
                                }
                                else {
                                    htmlStr = $('<td class="weekend"></td>');
                                   
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                }
                            }
                            else if (item.dateStr != today) {
                                htmlStr = $('<td></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                            } else {
                                htmlStr = $('<td class="selected"></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                            }
                            week5.append(htmlStr);
                        }
                    });
                    $.each(result.Week6, function (i, item) {
                        var htmlStr = null;
                        if (jQuery.isEmptyObject(item)) {
                            htmlStr = $('<td></td>');
                            week6.append(htmlStr);
                        } else {
                            if (item.daycolumn == 0 || item.daycolumn == 6) {
                                if (item.dateStr == today) {
                                    htmlStr = $('<td class="weekend selected"></td>');
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                }
                                else {
                                    htmlStr = $('<td class="weekend"></td>');
                                    
                                    htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                    if (item.holidayname.length > 0) {
                                        item.holidayname.forEach(function (holiday) {
                                            htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                        });
                                    }
                                    htmlStr.append('</a>');
                                }
                            }
                            else if (item.dateStr != today) {
                                htmlStr = $('<td></td>');
                                
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                            } else {
                                htmlStr = $('<td class="selected"></td>');
                               
                                htmlStr.append('<a   id=' + item.dateStr + ' class="dt"><h3>' + item.dtDay + '</h3>');
                                if (item.holidayname.length > 0) {
                                    item.holidayname.forEach(function (holiday) {
                                        htmlStr.append(' <span class="label label-primary" value="' + holiday.HolidayCalendarId + '">' + holiday.HolidayTypeName + '</span>');
                                    });
                                }
                                htmlStr.append('</a>');
                            }
                            week6.append(htmlStr);
                        }
                    });

                    $("#component-table").trigger("update");
                } else {
                    alertMsg('Oops, errors occur in retrieving calender');
                }
            }
        });
    });
});



function getTodayDate() {
    var localdate = new Date;
    var localday = localdate.getDate();
    //    if (localday < 10) {
    //        localday = '0' + localday;
    //    }
    var localmonth = localdate.getMonth() + 1;
    //    if (localmonth < 10) {
    //        localmonth = '0' + localmonth;
    //    }
    var localyear = localdate.getFullYear();

    var local = localmonth + "/" + localday + "/" + localyear;
    return local;
}


//function alertMsg(text) {
//    $("#dialog-modal").dialog({
//        title: text,
//        modal: true
//    });
//}


function getMonth(m) {
    var month;
    switch (m) {
        case "1":
            month = "बैशाख";
            return month;
            break;
        case "2":
            month = "जेठ";
            return month;
            break;
        case "3":
            month = "असार";
            return month;
            break;
        case "4":
            month = "साउन";
            return month;
            break;
        case "5":
            month = "भदौ";
            return month;
            break;
        case "6":
            month = "असोज";
            return month;
            break;
        case "7":
            month = "कार्तिक";
            return month;
            break;
        case "8":
            month = "मंसिर";
            return month;
            break;
        case "9":
            month = "पुस";
            return month;
            break;
        case "10":
            month = "माघ";
            return month;
            break;
        case "11":
            month = "फागुन";
            return month;
            break;
        case "12":
            month = "चैत";
            return month;
            break;
        default:
            month = "non";
            return month;
            break;
    }
}




