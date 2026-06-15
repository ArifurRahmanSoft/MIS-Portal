 
app.directive('enterAsTab', function () {
    return function (scope, element, attrs) {


        element.bind("blur", function (event) {
            event.preventDefault();
            element.parent('td').parent('tr').removeClass('rowselected'); 
        });


        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                event.preventDefault(); 
                var elementTitle = attrs.title; 
                var elementAlt = attrs.alt; // 0 for details table and 1 for master table  
                if (elementAlt == "0")
                { 
                    var elementToFocus = element.parent('td').parent('tr').next('tr').find('input')[elementTitle];;
                    if (angular.isDefined(elementToFocus))
                        elementToFocus.focus();
                    
                    element.parent('td').parent('tr').next('tr').prevAll('tr').removeClass('rowselected');
                    element.parent('td').parent('tr').next('tr').nextAll('tr').removeClass('rowselected');
                    element.parent('td').parent('tr').next('tr').addClass('rowselected'); 
                }
               
            }
        });
    };
});