var ModalDocumentManager = (function(){
    function ModalDocumentManager() { }
    ModalDocumentManager.DeadLine = function (id) {

        Items_2__DeadLine
        var checkbox = document.getElementById("Items_" + id + "__HasDeadline");
        var Input = document.getElementById("Items_" + id + "__DeadLine");
      
        checkbox.parentElement.style.display = "none";
        Input.parentElement.style.display = "initial";
    }
    ModalDocumentManager.Save = function (callback) {
        callback();
        //window.postMessage("Save", "DocumentManager");
        //window.addEventListener("message", receiveMessage, false);

        //function receiveMessage(event) {
        //    var origin = event.origin || event.originalEvent.origin; // For Chrome, the origin property is in the event.originalEvent object.
        //    if (origin !== "http://example.org:8080")
        //        return;

        //    // ...
        //}
    };
    ModalDocumentManager.Delete = function(sender,nr){
        document.getElementById("row_"+nr).style.display = "none";
        document.getElementById("Items_"+nr+"__PublishDate").remove(); //want this as null on server side i.e unpublish or remove if not saved to database...
    }
    return ModalDocumentManager;
})();

