class Utils {
  static String truncate(String s, [int max = 50]) {
    return (s.length <= max) ? s : '${s.substring(0, max)}...';
  }
}
