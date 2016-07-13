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

    ModalDocumentManager.Click = function (base,type,id,title) {
        var iframe = document.getElementById("DocumentManagerIframe");
        iframe.contentWindow.location = base + "Document/Add/?EntityId=" + id + "&entityType=" + type;
       
        if (title !== undefined && title !== null) {
            document.getElementById("myModalLabel").innerText = title;
        }
        
    };


    ModalDocumentManager.Save = function (callback) {   
        var iframe = document.getElementById("DocumentManagerIframe");
        var iframeContent = iframe.contentWindow;
        var iframedoc = iframeContent.document;       
        var done = iframedoc.getElementById("done");
        done.value = "true";
        iframe.onload = function () {
            iframe.onload = undefined;
            callback();
        }
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

