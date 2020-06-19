public static class Utility
{
	public static float convertRange(float old_value, float old_min, float old_max, float new_min, float new_max) {
		return (((old_value - old_min) * (new_max - new_min)) / (old_max - old_min)) + new_min;
	}

	public static float convertRangeOne(float old_value, float new_min, float new_max) {
		var new_range = (new_max - new_min);
		var new_value = ((old_value) * new_range) + new_min;
		return new_value;
	}
}
