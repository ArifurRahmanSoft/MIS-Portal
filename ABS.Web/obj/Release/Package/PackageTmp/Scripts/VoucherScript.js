$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $(".datetimepicker11").datepicker({
        format: "dd/mm/yyyy",
        todayHighlight: true,
        autoclose: true
    });

    $('#btnLedger5').click(function () {
        var url = $('#btnLedger5').data('url');
        $.get(url,
            function (data) {
                $('#modalAc5').html(data);

                $('#modalAc5').modal('show');
            });
    });


    //hide total debit and credit amount
    $(".total").hide();

    //hide table head
    $(".theadLedger").hide();


    //reset button click event
    $("#reset").on('click',
        function () {

            $('div .form-group').removeClass('has-error');

            $("span .text-danger").remove();

            $('#voucherForm').trigger("reset");
            $("#VoucherNo").val("NEW");


            $("input[id^='AC5Id']").select2("val", "");
            $("input[id^='CostCenterId']").select2("val", "");
            $("#debit,#credit").val("0");
            $('#ledgerBalance').text('');
            $(".theadLedger").hide();
            $(".total").hide();

            //Delete table
            //alert($("#voucherDetails tbody"))
            $("#voucherDetails tbody").find("tr").remove();
            //$("#voucherDetails tbody").empty();
            //console.log($("#voucherDetails tbody").find("tr:gt(1)"));
            $(".datetimepicker11").datepicker('setDate', new Date());

            $('.field-validation-error')
                .removeClass('field-validation-error')
                .addClass('field-validation-valid');

            $('.input-validation-error')
                .removeClass('input-validation-error')
                .addClass('valid');


        });
    //end reset button



    $("#debit").on('keyup mouseup',
        function () {
            sumDebit1('debit', 'credit');
        });
    $("#credit").on('keyup mouseup',
        function () {
            sumCredit1('credit', 'debit');
        });




    $('.debit,.credit').each(function (index) {
        if ($(this).val() == 0) {
            $(this).attr("readonly", true);
        }

    });

    //Add button click event
    $('#clear').click(function () {


        $("input[id^='AC5Id']").select2("val", "");
        $("input[id^='CostCenterId']").select2("val", "");
        $("#debit,#credit").prop('disabled', false).val("0");

        $('#ledgerBalance').text('');
        $('span.error').css('visibility', 'hidden');
    });

    $('#add,#debit,#credit').keypress(function (e) {
        if (e.which == 13) { //Enter key pressed
            $('#add').click(); //Trigger search button click event
        }
    });
    //Add button click event
    $('#add').click(function () {
        //validation and add order items

        var isAllValid = true;
        var AC5Id = $('#AC5Id').val();
        if (AC5Id == "0" || AC5Id == "") {
            isAllValid = false;
            $('#AC5Id').siblings('span.error').css('visibility', 'visible');
        } else {
            $('#AC5Id').siblings('span.error').css('visibility', 'hidden');
        }
        var costCenterId = $('#CostCenterId').val();
        //if (costCenterId == "0" || costCenterId == "") {
        //    isAllValid = false;
        //    $('#CostCenterId').siblings('span.error').css('visibility', 'visible');
        //} else {
        //    $('#CostCenterId').siblings('span.error').css('visibility', 'hidden');
        //}
        var debitValue = $('#debit').val().trim();
        var creditValue = $('#credit').val().trim();
        if ((debitValue == '' || debitValue == '0') && (creditValue == '' || creditValue == '0')) {
            isAllValid = false;
            $('#debit').siblings('span.error').css('visibility', 'visible');
            $('#credit').siblings('span.error').css('visibility', 'visible');

        } else {
            $('#debit').siblings('span.error').css('visibility', 'hidden');
            $('#credit').siblings('span.error').css('visibility', 'hidden');

        }


        if (isAllValid) {

            $(".theadLedger").show();
            var $newRow = $('#mainrow').clone().removeAttr('id');
            $('input', $newRow).removeAttr("tabindex");
            $('.accountLedger', $newRow).val(AC5Id).removeAttr("tabindex");
            $('.costCenter', $newRow).val(costCenterId).removeAttr("tabindex");
            //if (isNaN(debitValue) || debitValue == '') {
            //    $('.debit', $newRow).val("0");
            //}
            //if (isNaN(creditValue) || creditValue == '') {
            //    $('.credit', $newRow).val("0");
            //}

            //Replace add button with remove button
            $('#add', $newRow).removeClass('btn-success').addClass('btn-danger').addClass("confirm-dialog");
            $('#add', $newRow).find("i").removeClass('icon-plus').addClass('icon-trash');
            $('#clear', $newRow).hide();
            $('#btnLedger5', $newRow).hide();

            $('#debit', $newRow).addClass('debit').attr("onkeyup", "return sumDebitAmount(this)");
            $('#credit', $newRow).addClass('credit').attr("onkeyup", "return sumCreditAmount(this)");
            //remove id attribute from new clone row
            $('#AC5Id,#CostCenterId,#debit,#credit,#add', $newRow).removeAttr('id');

            $('span.error', $newRow).remove();
            //append clone row
            $('#voucherDetails').append($newRow);

            //show total debit credit field
            $(".total").show();

            //sum total debit and credit
            SumTotalDebitCreditAmount();


            //clear select data
            $('#AC5Id,#CostCenterId').select2("val", '');
            $('#debit,#credit').val('0');
            $('#debit,#credit').prop('disabled', false);

            $('.debit,.credit').attr("readonly", false);
            $('.debit,.credit').attr("disabled", false);

            $('.debit,.credit').each(function (index) {
                if ($(this).val() == 0) {
                    $(this).attr("readonly", true);
                }

            });

            $("#ledgerBalance").empty();
            $('#voucherDetailsError').empty();
            //  $("#AC5Id").select2('open');
        }

    });



    $(document).on('click',
        '.confirm-dialog',
        function () {
            var currentRow = $(this).closest('tr');
            bootbox.confirm("Are you sure want to delete this row?",
                function (confirmed) {
                    if (confirmed == true) {
                        //remove button click event
                        currentRow.remove();
                        var tableLength = $("#voucherDetails tbody").find('tr').length;
                        if (tableLength == 0) {
                            $(".total").hide();
                            $(".theadLedger").hide();
                        }

                        //sum total debit and credit
                        SumTotalDebitCreditAmount();

                    }

                });
        });

});

function DeleteTableRow() {
    var tableLength = $("#voucherDetails tbody").find('tr').length;

    if (tableLength > 0) {
        $('#voucherDetails tbody tr:last').remove();
        Command: toastr["warning"]("table row deleted!");
    }
    tableLength = $("#voucherDetails tbody").find('tr').length;
    if (tableLength == 0) {
        $(".total").hide();
        $(".theadLedger").hide();
    }
    //sum total debit and credit
    SumTotalDebitCreditAmount();
}

function sumDebitAmount($this) {

    var debitValue = $($this).val();
    var txtCredit = $($this).closest('td').next('td').find(".credit");
    if (debitValue.length > 0 && debitValue != 0) {
        $(txtCredit).val("0");
        $(txtCredit).attr("readonly", true);

    }
    else {
        $(txtCredit).attr("readonly", false);;
        $($this).val("");
    }

    SumTotalDebitCreditAmount();

}

function sumCreditAmount($this) {
    var creditValue = $($this).val();
    var txtDebit = $($this).closest('td').prev('td').find(".debit");
    if (creditValue.length > 0 && creditValue != 0) {
        $(txtDebit).val("0");
        $(txtDebit).prop('readonly', true);

    }
    else {
        $(txtDebit).attr("readonly", false);;
        $($this).val("");
    }
    SumTotalDebitCreditAmount();

}


$("input.debit:number").on('keyup mouseup',
    function () {


        //$(this).closest('td').prev('td').text();
        $(this).closest('td').next('td').find(".credit").val()

        sumDebit1(this, 'credit');
    });
$(".credit").on('keyup mouseup',
    function () {

        sumCredit1(this, 'debit');
    });


shortcut.add("Ctrl+s", function () {
    $('#save').click();
});
shortcut.add("Alt+Delete", function () {
    DeleteTableRow();
});
shortcut.add("Ctrl+1", function () {
    $('#reset').click();
}); shortcut.add("Alt+Up", function () {
    $('#clear').click();
});
shortcut.add("Alt+Down", function () {
    $("#AC5Id").select2('open');
});
shortcut.add("Alt+Left", function () {
    $('#Show').click();
});
shortcut.add("Alt+n", function () {
    $('#btnLedger5').click();
});
shortcut.add("Alt+d", function () {
    $('#debit').focus();
});
shortcut.add("Alt+c", function () {
    $('#credit').focus();
});

