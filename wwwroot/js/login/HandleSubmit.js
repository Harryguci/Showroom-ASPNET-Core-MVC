async function HandleSubmit(event) {
     event.preventDefault();

    if (!confirm("Bạn muốn đăng nhập?")) return;

    var username = document.querySelector(`form input[name="username"]`).value;
    var password = document.querySelector(`form input[name="password"]`).value;

    if (username.length < 3) {
        alert("Tên đăng nhập quá ngắn.");
        return;
    }
    if (password.length < 6) {
        alert("Mật khẩu phải trên 6 ký tự.")
        return;
    }
    var url = "https://localhost:3000/Auth/Login";

    var data = {
        username: username,
        password: password
    };

    await fetch(url, {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },

        //make sure to serialize your JSON body
        body: JSON.stringify(data)
    }).then(response => response.json())
        .then(response => {
            if (response.access_token)
            {
                localStorage.setItem("access_token", response.access_token)
                // cookie.setItem("access_token", response.access_token)
                window.location = '/Home/Index'
            }
            else if (response.error) {
                document.querySelector(".form-helper").innerHTML = response.error;
            }
        });
}