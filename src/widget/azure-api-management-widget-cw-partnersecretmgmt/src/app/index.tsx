import { useEffect, useState } from "react"
import { useRequest, useSecrets, useValues } from "../hooks"
import { AppReg, Utility, Backend, SecretContext } from "../common"

const App = () => {
  const secrets = useSecrets()
  const [appRegList, setAppRegList] = useState<AppReg[]>([])
  const [description, setDescription] = useState<string>("")
  const captionStyle = { width: 200 }
  const valueStyle = { backgroundColor: "#6B8DC6", width: 300}

  const createContext = (): SecretContext => {
    return {
      managementApiUrl: secrets.managementApiUrl,
      apiVersion: secrets.apiVersion,
      token: secrets.token,
      userId: secrets.userId,
      hostname: secrets.parentLocation.hostname,
      origin: secrets.parentLocation.origin
    }
  }

  useEffect(() => {
    setTimeout(async () => { 
      const appRegs = await Backend.list("/list", createContext());
      setAppRegList(appRegs)     
    }, 1);
  }, [])

  const onGenerate = async () => {
    const appRegs = await Backend.generate("/generate", createContext(), description)
    setAppRegList(appRegs)
    setDescription("")
  }


  return (
    <>
      <h4>Partner Secret Management</h4>
      {appRegList.map((appReg, index) => (
            <div key={index} className="form-group">
              <table>
                <tr>
                  <td style={captionStyle}><label className="form-label">Client ID: </label></td>
                  <td style={valueStyle}><b>{appReg.clientId}</b></td>
                  <td style={captionStyle}><label className="form-label">Description: </label></td>
                  <td style={valueStyle}><b>{appReg.displayName}</b></td>
                </tr>
                <tr>
                  <td  style={captionStyle}><label className="form-label">Start time: </label></td>
                  <td style={valueStyle}><b>{Utility.convert(appReg.startDateTime) }</b></td>
                  <td style={captionStyle}><label className="form-label">End time: </label></td>
                  <td style={valueStyle}><b>{Utility.convert(appReg.endDateTime)}</b></td>
                </tr>
                {
                  appReg.clientSecret && 
                  <tr>
                    <td style={captionStyle}><label className="form-label">Client Secret: </label></td>
                    <td style={valueStyle}><b>{appReg.clientSecret}</b></td>
                    <td style={captionStyle} colSpan={2}>Please copy the secret, you won't see it again!</td>
                  </tr>
                }
              </table>
            </div>
          ))}

        <h4>Create New secret</h4>
        <div className="form-group">
           <label htmlFor="description" className="form-label">Description</label>
           <input type="text" className="form-control" name="description" placeholder="Please enter description" defaultValue="" value={description} onChange={(e) => setDescription(e.target.value)} />
        </div>
        <div className="form-group">
          <button type="button" onClick={onGenerate} className="button button-primary">Generate</button>
        </div>          
    </>
  )
}

export default App




      // {/* <div className="form-group">
      //   <label htmlFor="email" className="form-label">{values.label1}</label>
      //   <input id="email" type="email" className="form-control" name="email" placeholder="example@contoso.com"
      //     defaultValue={defaultEmail} />
      // </div> */}

      // {/* <div className="form-group height-fill flex-columns-container">
      //   <label htmlFor="message" className="form-label">{values.label2}</label>
      //   <textarea id="message" className="form-control flex-grow" name="message" placeholder={values.placeholder}>
      //   </textarea>
      // </div> */}
      // {/* <div className="form-group">
      //   <button type="submit" className="button button-primary">Submit</button>
      // </div> */}

      

  // const [defaultEmail, setDefaultEmail] = useState<string | undefined>()

  // useEffect(() => {
  //   if (!userId) {
  //     setDefaultEmail("")
  //     return
  //   }

  //   request(`/users/${userId}`)
  //     .then(e => e.json())
  //     .then(({properties}) => setDefaultEmail(properties.email))
  //     .catch(e => {
  //       console.error("Could not prefill the email address!", e)
  //       setDefaultEmail("")
  //     })
  // }, [userId, request])

  // if (defaultEmail == undefined) return <div className="loading"></div>



  //   //const values = useValues()
  // //const {userId} = useSecrets()
  // const userSecrets = useSecrets()
  // //const request = useRequest()

  // console.log(userSecrets)