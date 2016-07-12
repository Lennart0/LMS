var ModalDocumentManager = (function(){
    function ModalDocumentManager() { }
    ModalDocumentManager.DeadLine = function (id) {

   
        var checkbox = document.getElementById("Items_" + id + "_Div_HasDeadline");
        var Input = document.getElementById("Items_" + id + "_Div_DeadLine");

        var Othercheckbox = document.getElementById("Items_" + id + "_Div_IsAssigmentSubmission");
        var OtherInput = document.getElementById("Items_" + id + "_Div_AssignmentId");
 
        Othercheckbox.style.display = "none";
        OtherInput.style.display = "none";

        checkbox.style.display = "none";
        Input.style.display = "initial";
    }


    ModalDocumentManager.ShowAssigmentOptions = function (id) {
 

        var checkbox = document.getElementById("Items_" + id + "_Div_HasDeadline");
        var Input = document.getElementById("Items_" + id + "_Div_DeadLine");
                                           

        var Othercheckbox = document.getElementById("Items_" + id + "_Div_IsAssigmentSubmission");
        var OtherInput = document.getElementById("Items_" + id + "_Div_AssignmentId");


        Othercheckbox.style.display = "none";
        OtherInput.style.display = "initial";

        checkbox.style.display = "none";
        Input.style.display = "none";
    }


    ModalDocumentManager.ReciveViaIframe = function () {
        /*

        window.addEventListener("Load", function () {

            window.postMessage("Save", "DocumentManager");
            window.addEventListener("message", receiveMessage, false);
            function receiveMessage(event) {
                var origin = event.origin || event.originalEvent.origin; // For Chrome, the origin property is in the event.originalEvent object.
                if (origin !== "http://localhost:49859/")
                    return;
                callback();
                window.removeEventListener("message", receiveMessage, false);
            }


        });
        */
    };

    ModalDocumentManager.Save = function (callback) {
      /*
        window.postMessage("Save", "DocumentManager");       
        window.addEventListener("message", receiveMessage, false);
        function receiveMessage(event) {
            var origin = event.origin || event.originalEvent.origin; // For Chrome, the origin property is in the event.originalEvent object.
            if (origin !== "http://localhost:49859/")
                return;
            callback();
            window.removeEventListener("message", receiveMessage, false);
        }*/

   
        var iframe = document.getElementById("DocumentManagerIframe");
        var iframeContent = iframe.contentWindow;
        var iframedoc = iframeContent.document;
       
        var done = iframedoc.getElementById("done");
        done.value = "true";


        setTimeout(function () { callback();   }, 3000);//todo make this work better... surely there must be a way of getting this to trigger on reload....
        iframedoc.forms[0].submit();
    
  
  
    };
    ModalDocumentManager.Delete = function (sender, nr) {
        if (confirm("Är du säker att du vill ta bort dokumentet?")) {
        document.getElementById("row_"+nr).style.display = "none";
        document.getElementById("Items_"+nr+"__PublishDate").remove(); //want this as null on server side i.e unpublish or remove if not saved to database...
        }
    }
    return ModalDocumentManager;
})();

