public static class Utility
{
	public static float convertRangeOne(float old_value, float new_min, float new_max) {
		var new_range = (new_max - new_min);
		var new_value = ((old_value) * new_range) + new_min;
		return new_value;
	}
}
