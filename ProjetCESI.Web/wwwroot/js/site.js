// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleResponse(elementId, count) {
    var text = document.getElementById('comments-' + elementId).innerText

    if (count > 1) {
        texte = "les " + count + " commentaires";
    }
    else {
        texte = "le commentaire";
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
        });
}