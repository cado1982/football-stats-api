/// <binding AfterBuild='scripts, styles' />
var gulp = require("gulp");
var merge = require("gulp-sequence");
var sass = require("gulp-sass");
var concat = require("gulp-concat");
var plumber = require("gulp-plumber");
var notify = require("gulp-notify");

gulp.task("scripts", function () {
    return gulp.src([
        'node_modules/jquery/dist/jquery.min.js',
        'node_modules/bootstrap/dist/js/bootstrap.bundle.min.js',
        'node_modules/qrcodejs/qrcode.min.js'
    ])
        .pipe(concat('vendor.min.js')) 
        .pipe(gulp.dest('wwwroot/lib'));
});

gulp.task("styles", function () {
    var sassSource = gulp.src("Styles/**/*.scss")
        .pipe(plumber({
            errorHandler: function (err) {
                notify.onError({
                    title: "Gulp error in " + err.plugin,
                    message: err.toString()
                })(err);
                this.emit('end');
            }
        }))

        var regular = sassSource.pipe(sass())
                                .pipe(concat("site.css"))
                                .pipe(gulp.dest("wwwroot/css"));
        var minified = sassSource.pipe(sass({outputStyle: "compressed"}))
                                 .pipe(concat("site.min.css"))
                                 .pipe(gulp.dest("wwwroot/css"));

        return merge(regular, minified);
});

gulp.task("styles:watch", function () {
    gulp.watch("Styles/**/*.scss", ['styles']);
});

gulp.task("build", ["scripts", "styles"]);
gulp.task("watch", ["styles:watch"]);