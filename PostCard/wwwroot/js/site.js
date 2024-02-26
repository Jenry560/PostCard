document.querySelector("#closeSection").addEventListener('click', function (e) {

    console.log(e.target.href)
    
    
    swal({
        title: 'Are you sure?',
        text: 'Are you sure do you want to quit the section?',
        icon: 'info',
        buttons: true,
        dangerMode: true
    }).then((confirmed) => {
        if (confirmed) {
            window.location.href = e.target.href
        }

    })
    e.preventDefault();
})

var likeButtons = document.querySelectorAll("#likeButton");



likeButtons.forEach(function (boton) {
    boton.addEventListener("click", function (event) {

        iconBottom = boton.querySelector(".likeIcon")
        let count = parseInt(iconBottom.innerHTML)

        if (iconBottom.style.color == 'blue') {
            iconBottom.style.color = 'black'
            iconBottom.innerHTML = count - 1
        } else {
            iconBottom.style.color = 'blue'
            iconBottom.innerHTML = count + 1
        }

        let postId = boton.getAttribute("data-postId");
        var xhr = new XMLHttpRequest();
        xhr.open("POST", `/Home/AddLike?postId=${postId}`, true);
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    console.log("Like agregado correctamente");
                } else {
                    console.error("Error al agregar el like");
                }
            }
        };
        console.log(postId)
        xhr.send(JSON.stringify({ postId: postId }));
    });
})



let formDelete = document.querySelectorAll("#Formdelete")

formDelete.forEach(function (form) {
    form.addEventListener("submit", function (e) {
        e.preventDefault()
        swal({
            title: 'Estas Seguro?',
            text: 'Estas seguro que desea eliminar este post',
            icon: 'info',
            buttons: true,
            dangerMode: true
        }).then((confirmed) => {
            if (confirmed) {
                this.submit();
            }

        })
    })
})
