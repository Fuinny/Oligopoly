# :question: How to add your own event
1. Open ```Events.xml``` file.
2. Find ```Event``` element.
3. Copy the following template and paste it below last ```Event``` element:

      ```xml
      <Event id="EVENT_ID">
        <Effect>EVENT_EFFECT</Effect>
        <Target>EVENT_TARGET</Target>
        <Title>EVENT_TITLE</Title>
        <Content>EVENT_CONTENT</Content>
      </Event>
      ```

5. Replace **EVENT_ID**, **EVENT_EFFECT**, **EVENT_TARGET**, **EVENT_TITLE** and **EVENT_CONTENT** with the appropriate values for your new event.
6. Save the changes to the ```Events.xml``` file.

> **Please note:**
>
> **EVENT_ID** - can be anything, make it so that it is easier for you to navigate in the xml file, **but** you need to use underscore between words.
> The default structure is: Company_Event, but you can use other structure.
>
> Example: ```ABC_Company_new_event```
>
> **EVENT_EFFECT** - can be any integer except zero.
>
> **EVENT_TARGET** - **must** match the name of the company to which the effect is to be applied. Obviously cannot be null or whitespace.
>
> Example: ```ABC Company Name```
>
> **EVENT_TITLE** and **EVENT_CONTENT** - can be anything, except null or whitespace.
>
> Example: ```ABC Company something happened``` and ```ABC Company long-long-long event content.```