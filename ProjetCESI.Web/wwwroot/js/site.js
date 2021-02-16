// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleResponse(elementId, count) {
    var text = document.getElementById('comments-' + elementId).innerText

    if (count > 1) {
        texte = "les " + count + " réponses";
    }
    else {
        texte = "la réponse";
    }

    if (text.includes("Masquer")) {
        document.getElementById('comments-' + elementId).innerText = "Afficher " + texte;
    }
    else {
        document.getElementById('comments-' + elementId).innerText = "Masquer " + texte;
    }

    $('#collapseResponse-' + elementId).collapse('toggle');
}

function resetEditor(element) {
    element.value = "";
}

function submitReponseForm(commentId, ressourceId, userId) {
    $.ajax({
        url: '/Commentaire/RepondreCommentaire',
        method: 'POST',
        data: {
            contenu: $('#response-editor-' + commentId).val(),
            ressourceId: ressourceId,
            utilisateurId: userId,
            commentaireParentId: commentId
        }
    })
        .done(function (result) {
            document.getElementById("list-commentaire").innerHTML = result;
            document.getElementById('envoyer-btn').disabled = true;
        });
}

function updateSubmitButton(commentId) {
    let value = $('#response-editor-' + commentId).val();

    if (value == '') {
        document.getElementById('response-submit-' + commentId).disabled = true;
    }
    else {
        document.getElementById('response-submit-' + commentId).disabled = false;
    }
}