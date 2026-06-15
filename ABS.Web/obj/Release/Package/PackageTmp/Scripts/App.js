(function (Select2Load, $, undefined) {
    'use strict';

    Select2Load.Name = 'Select2Loadlication';

    Select2Load.init = function () {
    };

    Select2Load.render = function () {
    };

    Select2Load.DropDownsLookup = function (lookupCode, lookupDescription, urlFetchCountries, multiple, pageSize) {
       
        $(lookupCode).select2({
            minimumInputLength: 0,
            multiple: multiple || false,
            quietMillis: 200,
            allowClear: true,
            ajax: {
                url: urlFetchCountries,
                dataType: 'json',
                type: "POST",
                data: function(term, page) {
                     return {
                         country: term,
                         pageSize: pageSize,
                         pageNum: page
                     };
                },
                results: function (data, page) {
                    //console.log(data);
                    var more = (page * pageSize) < data.total;
                    //return { results: data.Results, more: more };
                    return {
                        results: $.map(data.Results,function (item) {
                            //var more = (page * pageSize) < data.total;
                            //return { results: data.Results, more: more };
                            return {
                                id: item.Code,
                                text: item.Description
                            };
                        }), more: more
                    };
                }
            },
            initSelection: function (item, callback) {
                if (!multiple) {
                    var id = item.val();
                    var text = item.data('option');
                    if (!text) {
                        text = "";
                    }
                    var data = { id: id, text: text };
                    callback(data);
                }
                else {
                    var data = [];
                    var items = item.val().split(',');
                    $.each(items, function (index, item) {
                        data.push({ id: item, text: item });
                    });
                    $(lookupCode).val('');
                    callback(data);
                }
            },
            formatResult: function (item) {
                return ('<div>' + item.id + ' - ' + item.text + '</div>');
            },
            formatSelection: function (item) {
                if (jQuery.isEmptyObject(item)) {
                    return ('');
                }
                if (lookupDescription) {
                    $(lookupDescription).val(item.text);
                }
                return (multiple ? item.id : item.text);
            },
            dropdownCssClass: "bigdrop",
            escapeMarkup: function (m) { return m; }
        }).on('change', function (e) {
            if ((!jQuery.isEmptyObject(e.removed) && (!e.added)) && (lookupDescription)) {
                $(lookupDescription).val('');
            }
        });
    };
    Select2Load.CascadeDropDownsLookup = function (lookupCode, lookupDescription, urlFetchCountries, multiple, pageSize,cascadeSelector) {
        var dropdownValue = $(cascadeSelector).value;
        $(lookupCode).select2({
            minimumInputLength: 0,
            multiple: multiple || false,
            quietMillis: 200,
            allowClear: true,
            ajax: {
                url: urlFetchCountries,
                dataType: 'json',
                type: "POST",
                data: function (term, page) {
                    return {
                        country: term,
                        pageSize: pageSize,
                        pageNum: page,
                        dropDown:dropdownValue
                    };
                },
                results: function (data, page) {
                    //console.log(data);
                    var more = (page * pageSize) < data.total;
                    //return { results: data.Results, more: more };
                    return {
                        results: $.map(data.Results, function (item) {
                            //var more = (page * pageSize) < data.total;
                            //return { results: data.Results, more: more };
                            return {
                                id: item.Code,
                                text: item.Description
                            };
                        }), more: more
                    };
                }
            },
            initSelection: function (item, callback) {
                if (!multiple) {
                    var id = item.val();
                    var text = item.data('option');
                    if (!text) {
                        text = "";
                    }
                    var data = { id: id, text: text };
                    callback(data);
                }
                else {
                    var data = [];
                    var items = item.val().split(',');
                    $.each(items, function (index, item) {
                        data.push({ id: item, text: item });
                    });
                    $(lookupCode).val('');
                    callback(data);
                }
            },
            formatResult: function (item) { return ('<div>' + item.id + ' - ' + item.text + '</div>'); },
            formatSelection: function (item) {
                if (jQuery.isEmptyObject(item)) {
                    return ('');
                }
                if (lookupDescription) {
                    $(lookupDescription).val(item.text);
                }
                return (multiple ? item.id : item.text);
            },
            dropdownCssClass: "bigdrop",
            escapeMarkup: function (m) { return m; }
        }).on('change', function (e) {
            if ((!jQuery.isEmptyObject(e.removed) && (!e.added)) && (lookupDescription)) {
                $(lookupDescription).val('');
            }
        });
    };

}(window.Select2Load = window.Select2Load || {}, jQuery));