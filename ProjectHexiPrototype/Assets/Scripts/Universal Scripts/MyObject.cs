using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyObject : MonoBehaviour
{
    // wiggle variables
    public bool wiggleBoolean;
    public AnimationCurve wiggleCurve;
    public float wiggleDuration;
    public float wiggleMultiplier;

    // private variables
    private Coroutine scaleRoutine;
    private Coroutine wiggleRoutine;
    private Coroutine rotationRoutine;
    private Coroutine transformRoutine;
    private bool continuousWiggleObject;

    public void Move2D(Vector3 moveVector, float speed)
    {   
        moveVector.Normalize();
        transform.position += (moveVector * speed);
    }

    public void MoveToPosition(Vector3 targetPos, float duration, bool smooth = true, bool local = false)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPos;

        // switch to local pos if indicated
        if (local)
        {
            startPos = transform.localPosition;
        }
        StartCoroutine(LerpPosition(startPos, endPos, duration, smooth, local));
    }

    public void MoveToTransform(Transform targetTransform, float duration, bool smooth = true, bool local = false)
    {
        Vector3 startPos = transform.position;
        // switch to local pos if indicated
        if (local)
        {
            startPos = transform.localPosition;
        }

        if (transformRoutine != null)
        {
            StopCoroutine(transformRoutine);
        }
        transformRoutine = StartCoroutine(LerpToTransform(targetTransform, startPos, duration, smooth, local));
    }

    public void RandomizePosition(float maxXChange, float maxYChange, float xPos = 0f, float yPos = 0f)
    {
        float newXPos = xPos;
        float newYPos = yPos;

        newXPos += Random.Range(maxXChange * -1f, maxXChange);
        newYPos += Random.Range(maxYChange * -1f, maxYChange);

        transform.localPosition = new Vector3(newXPos, newYPos, 0f);
    }

    public void ChangeScale(float targetScale, float duration)
    {
        // end prev coroutine if not null
        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
        }
        scaleRoutine = StartCoroutine(LerpScale(transform.localScale.x, targetScale, duration));
    }

    public void ChangeScale(Vector2 targetScale, float duration)
    {
        // end prev coroutine if not null
        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
        }
        scaleRoutine = StartCoroutine(LerpScale(transform.localScale, targetScale, duration));
    }

    public void SquishyChangeScale(float targetScale1, float targetScale2, float duration1, float duration2)
    {
        // end prev coroutine if not null
        StartCoroutine(SquishyChangeScaleRoutine(targetScale1, targetScale2, duration1, duration2));
    }

    public void ChangeImageAlpha(float targetAlpha, float duration)
    {
        StartCoroutine(LerpImageAlpha(targetAlpha, duration));
    }

    public void ChangeTextMeshAlpha(float targetAlpha, float duration)
    {
        StartCoroutine(LerpTextMeshAlpha(targetAlpha, duration));
    }

    public void ChangeCanvasGroupAlpha(float targetAlpha, float duration)
    {
        StartCoroutine(LerpCanvasGroupAlpha(targetAlpha, duration));
    }

    public void ChangeRotation(float targetAngle, float duration, bool smoothLerp = false)
    {
        if (rotationRoutine != null)
            StopCoroutine(rotationRoutine);

        rotationRoutine = StartCoroutine(LerpRotationRoutine(targetAngle, duration, smoothLerp));
    }

    public void ChangeRotation(Vector3 rotation3, float duration, bool smoothLerp = false)
    {
        if (rotationRoutine != null)
            StopCoroutine(rotationRoutine);

        // get negative angles
        rotation3.x = (rotation3.x > 180) ? rotation3.x - 360 : rotation3.x;
        rotation3.y = (rotation3.y > 180) ? rotation3.y - 360 : rotation3.y;
        rotation3.z = (rotation3.z > 180) ? rotation3.z - 360 : rotation3.z;

        rotationRoutine = StartCoroutine(LerpRotationRoutine(rotation3, duration, smoothLerp));
    }

    public void SetTextMeshText(string text)
    {
        // get text mesh component
        TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
    }

    public void SetTextMeshAlpha(float alpha)
    {
        // get text mesh component
        TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
    }

    public void SetCanvasGroupAlpha(float alpha)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = alpha;
    }

    public void SetImageAlpha(float newAlpha)
    {
        // get image component
        Image image = GetComponent<Image>();
        // stop if image is null
        if (image == null)
        {
            return;
        }
        // set image alpha
        image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
    }

    public void SetImageColor(Color newColor)
    {
        // get image component
        Image image = GetComponent<Image>();
        // set new color without affecting alpha value
        newColor.a = image.color.a;
        image.color = newColor;
    }

    public void SetRectTransformWidth(float width)
    {
        // get rect transform component
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform)
        {
            Vector2 prevSizeDelta = rectTransform.sizeDelta;
            rectTransform.sizeDelta = new Vector2(width, prevSizeDelta.y);
        }
    }

    public void WiggleObjectOnce()
    {
        continuousWiggleObject = false;
        wiggleRoutine = StartCoroutine(WiggleRoutine());
    }

    public void ToggleObjectWiggle(bool opt, bool randomStart = false)
    {
        if (opt == continuousWiggleObject)
            return;
        continuousWiggleObject = opt;

        if (continuousWiggleObject)
        {
            if (randomStart)
            {
                StartCoroutine(RandomStartObjectWiggle());
            }
            else 
            {
                wiggleRoutine = StartCoroutine(WiggleRoutine());
            }
        }
    }

    private IEnumerator RandomStartObjectWiggle()
    {
        float random = Random.Range(0f, wiggleDuration);
        yield return new WaitForSeconds(random);
        wiggleRoutine = StartCoroutine(WiggleRoutine());
    }

    public void ChangeCameraFOV(float targetFOV, float duration, bool smoothLerp = false)
    {
        StartCoroutine(ChangeCameraFOVRoutine(targetFOV, duration, smoothLerp));
    }
    private IEnumerator ChangeCameraFOVRoutine(float targetFOV, float duration, bool smoothLerp)
    {
        // get camera component
        Camera camera = GetComponent<Camera>();
        // return if camera component is null
        if (!camera)
        {
            yield break;
        }

        float startFOV = camera.fieldOfView;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                camera.fieldOfView = targetFOV;
                break;
            }
            float tempFOV = 0f;

            if (smoothLerp)
            {
                tempFOV = Mathf.Lerp(startFOV, targetFOV, Mathf.SmoothStep(0f, 1f, timer / duration));
            }
            else
            {
                tempFOV = Mathf.Lerp(startFOV, targetFOV, timer / duration);
            }
            
            camera.fieldOfView = tempFOV;
            yield return null;
        }
    }

    // ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- //

    private IEnumerator WiggleRoutine()
    {
        Vector3 startRotation = transform.rotation.eulerAngles;
        float timer = 0f;
        while (timer < wiggleDuration)
        {
            timer += Time.deltaTime;
            var quat = Quaternion.Euler(startRotation.x, startRotation.y, startRotation.z + wiggleCurve.Evaluate(timer) * wiggleMultiplier);
            transform.rotation = quat;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(startRotation);

        if (continuousWiggleObject)
        {
            yield return new WaitForSeconds(0.1f);
            wiggleRoutine = StartCoroutine(WiggleRoutine());
        }
    }

    private IEnumerator LerpImageAlpha(float targetAlpha, float duration)
    {
        // get image component
        Image image = GetComponent<Image>();
        // stop if image is null
        if (image == null)
        {
            yield break;
        }
        float startAlpha = image.color.a;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // linear interpolation between two floats
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0f, 1f, timer / duration));
            // set new alpha
            image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
            
            yield return null;
        }
    }

    private IEnumerator LerpTextMeshAlpha(float targetAlpha, float duration)
    {
        // get image component
        TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
        // stop if image is null
        if (textMesh == null)
        {
            yield break;
        }
        float startAlpha = textMesh.color.a;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // linear interpolation between two floats
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0f, 1f, timer / duration));
            // set new alpha
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, currentAlpha);
            
            yield return null;
        }
    }

    private IEnumerator LerpCanvasGroupAlpha(float targetAlpha, float duration)
    {
        // get image component
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        // stop if image is null
        if (canvasGroup == null)
        {
            yield break;
        }
        float startAlpha = canvasGroup.alpha;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // linear interpolation between two floats
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0f, 1f, timer / duration));
            // set new alpha
            canvasGroup.alpha = currentAlpha;
            
            yield return null;
        }
    }

    private IEnumerator SquishyChangeScaleRoutine(float targetScale1, float targetScale2, float duration1, float duration2)
    {
        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
        }
        scaleRoutine = StartCoroutine(LerpScale(transform.localScale.x, targetScale1, duration1));
        yield return new WaitForSeconds(duration1);
        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
        }
        scaleRoutine = StartCoroutine(LerpScale(transform.localScale.x, targetScale2, duration2));
        // yield return new WaitForSeconds(duration2);
    }

    private IEnumerator LerpScale(float startScale, float endScale, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // linear interpolation between two floats
            float currentScale = Mathf.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, timer / duration));
            // set new scale
            this.transform.localScale = new Vector3(currentScale, currentScale, 1f);
            
            yield return null;
        }
    }

    private IEnumerator LerpScale(Vector2 startScale, Vector2 endScale, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // linear interpolation between two floats
            Vector2 currentScale = Vector2.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, timer / duration));
            // set new scale
            this.transform.localScale = currentScale;
            
            yield return null;
        }
    }

    private IEnumerator LerpPosition(Vector3 startPos, Vector3 endPos, float duration, bool smooth, bool local)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // linear interpolation between two positions
            Vector3 currentPos;
            if (smooth)
            {   
                currentPos = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, timer / duration));
            }   
            else
            {
                currentPos = Vector3.Lerp(startPos, endPos, timer / duration);
            }
            
            // use local pos if indicated
            if (local)
            {
                this.transform.localPosition = new Vector3(currentPos.x, currentPos.y, currentPos.z);
            }
            else
            {
                this.transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);
            }
            
            yield return null;
        }

        // set target position
        if (local)
        {
            this.transform.localPosition = new Vector3(endPos.x, endPos.y, endPos.z);
        }
        else
        {
            this.transform.position = new Vector3(endPos.x, endPos.y, endPos.z);
        }
    }  

    private IEnumerator LerpToTransform(Transform targetTransform, Vector3 startPos, float duration, bool smooth, bool local)
    {
        float timer = 0f;
        Vector3 targetPosition = targetTransform.position;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // linear interpolation between two positions
            Vector3 currentPos;
            if (smooth)
            {   
                currentPos = Vector3.Lerp(startPos, targetPosition, Mathf.SmoothStep(0f, 1f, timer / duration));
            }   
            else
            {
                currentPos = Vector3.Lerp(startPos, targetPosition, timer / duration);
            }
            
            // use local pos if indicated
            if (local)
            {
                this.transform.localPosition = new Vector3(currentPos.x, currentPos.y, currentPos.z);
            }
            else
            {
                this.transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);
            }
            
            yield return null;
        }

        // set target position
        if (local)
        {
            this.transform.localPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        }
        else
        {
            this.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        }
    }

    private IEnumerator LerpRotationRoutine(float targetAngle, float duration, bool smoothLerp)
    {
        float startRotation = transform.localRotation.eulerAngles.z;
        float timer = 0f;

        float targetPlus360 = targetAngle + 360f;
        float targetMinus360 = targetAngle - 360f;

        // determine if there is a faster way to target angle
        if (Mathf.Abs(targetPlus360 - startRotation) < Mathf.Abs(targetAngle - startRotation))
            targetAngle = targetPlus360;
        if (Mathf.Abs(targetMinus360 - startRotation) < Mathf.Abs(targetAngle - startRotation))
            targetAngle = targetMinus360;

        while (true)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, targetAngle);
                break;
            }

            float tempAngle = 0f;

            if (smoothLerp)
            {
                tempAngle = Mathf.Lerp(startRotation, targetAngle, Mathf.SmoothStep(0f, 1f, timer / duration));
            }
            else
            {
                tempAngle = Mathf.Lerp(startRotation, targetAngle, timer / duration);
            }
            
            transform.localRotation = Quaternion.Euler(0f, 0f, tempAngle);
            
            yield return null;
        }
    }

    private IEnumerator LerpRotationRoutine(Vector3 targetRotation, float duration, bool smoothLerp)
    {
        Vector3 startRotation = transform.localRotation.eulerAngles;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                transform.localRotation = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
                break;
            }

            Vector3 tempRotation = Vector3.zero;

            if (smoothLerp)
            {
                tempRotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetRotation), Mathf.SmoothStep(0f, 1f, timer / duration)).eulerAngles;
                //tempRotation = Vector3.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, timer / duration));
            }
            else
            {
                tempRotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetRotation), timer / duration).eulerAngles;
                //tempRotation = Vector3.Lerp(startRotation, targetRotation, timer / duration);
            }
            
            transform.localRotation = Quaternion.Euler(tempRotation);
            
            yield return null;
        }
    }
}
