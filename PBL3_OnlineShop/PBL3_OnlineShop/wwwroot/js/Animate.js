document.addEventListener("DOMContentLoaded", function () {
    // Gắn hàm vào window để HTML gọi được
    window.changeMainImage = function (thumbnail) {
        const mainImage = document.getElementById("mainImage");
        mainImage.src = thumbnail.src;

        const thumbnails = document.querySelectorAll('.thumbnail');
        thumbnails.forEach(t => t.classList.remove('active'));
        thumbnail.classList.add('active');
    };

    // Xử lý sự kiện cho các nút chart-btn
    document.querySelectorAll('.chart-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            // Xóa active khỏi tất cả các nút
            document.querySelectorAll('.chart-btn').forEach(b => b.classList.remove('active'));

            // Thêm active cho nút bấm
            this.classList.add('active');

            const type = this.dataset.type;  // Lấy type từ data-type của nút bấm
            loadChartData(type);  // Gọi hàm loadChartData với type truyền vào
        });
    });

    // Hàm để lấy dữ liệu biểu đồ từ server
    function loadChartData(type = 'monthly') {
        fetch(`/Admin/Statistic/GetRevenueData?type=${type}`)
            .then(res => res.json())
            .then(data => {
                renderChart(data.labels, data.values);  // Gọi hàm renderChart để vẽ biểu đồ
            })
            .catch(err => console.error("Error loading chart data:", err)); // In ra lỗi nếu có
    }

    // Hàm vẽ biểu đồ
    function renderChart(labels, values) {
        const ctx = document.getElementById('revenueChart').getContext('2d');

        if (window.chart) { // Nếu biểu đồ đã tồn tại, xóa đi và vẽ lại
            window.chart.destroy();
        }

        window.chart = new Chart(ctx, {
            type: 'line',  // Chọn kiểu biểu đồ (line, bar, pie, scatter, .. cả đống)
            data: {
                labels: labels,
                datasets: [{
                    label: 'Revenue',
                    data: values,
                    borderColor: 'rgba(75, 192, 192, 1)',  // Màu đường biểu đồ
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    fill: true
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    // Load mặc định (monthly)
    loadChartData();
});
