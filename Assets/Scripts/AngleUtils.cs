using UnityEngine;

public class AngleUtils
{
    // limit rotation on the x and z axis
    public static Vector3 LimitRotation(Vector3 vector, float maxAngle)
    {
        // normalize angles
        float nx = normalizeAngle(vector.x);
        float ny = normalizeAngle(vector.y);
        float nz = normalizeAngle(vector.z);

        // x and z rotation hypotenuse (basically the total rotation between the two)
        float mag = Mathf.Sqrt(Mathf.Pow(nx, 2) + Mathf.Pow(nz, 2));
        if (mag > maxAngle)
        {
            // target rotation greater than limit; limit rotation
            // angle between x and z
            float angle = Mathf.Atan2(nz, nx);
            // compute limited x and z angles
            return new Vector3(Mathf.Cos(angle) * maxAngle, vector.y, Mathf.Sin(angle) * maxAngle);
        }
        else
        {
            // no limitation needed
            return vector;
        }
    }

    public static Vector3 RotateVectorAroundVector(Vector3 vector, Vector3 subject)
    {
        Vector3 yOnly = new Vector3(0, subject.y, 0);

        // rotate the rotation control by the camera angle
        // (in other words, make sure left is always left and right is always right from the player's perspective)
        return Quaternion.Euler(yOnly) * vector;
    }

    // Adjust the provided angle to be within a -180 to 180.
    public static float normalizeAngle(float angle)
    {
        if (angle > 180.0f)
        {
            return angle - 360.0f;
        }
        if (angle < -180.0f)
        {
            return angle + 360.0f;
        }
        return angle;
    }
}
