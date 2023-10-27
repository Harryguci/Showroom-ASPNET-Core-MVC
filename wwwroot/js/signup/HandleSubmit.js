async function HandleSubmit(event) {
    event.preventDefault();

    if (!confirm("Bạn muốn đăng ký?")) return;

    var username = document.querySelector(`form input[name="username"]`).value;
    var password = document.querySelector(`form input[name="password"]`).value;
    var confirmPassword = document.querySelector(`form input[name="confirmPassword"]`).value;

    if (username && username.length < 3) {
        alert("Tên đăng nhập quá ngắn.");
        return;
    }
    if (password && password.length < 6) {
        alert("Mật khẩu phải trên 6 ký tự.");
        return;
    }
    if (password !== confirmPassword) {
        alert("Mật khẩu và xác nhận mật khẩu không khớp.");
        return;
    }

    var url = "https://localhost:3000/Accounts/SignUp"; // Đảm bảo URL là đúng cho API đăng ký của bạn

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

        // Đảm bảo serialize đúng dữ liệu JSON
        body: JSON.stringify(data)
    }).then(response => response.json())
        .then(response => {
            if (response.success) {
                alert("Đăng ký thành công.");
                window.location = '/Account/Login'; // Chuyển hướng sau khi đăng ký thành công
            } else if (response.error) {
                document.querySelector(".form-helper").innerHTML = response.error;
            }
        });
}
